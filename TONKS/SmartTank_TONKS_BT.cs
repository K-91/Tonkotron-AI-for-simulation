using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class SmartTank_TONKS_BT : AITank
{
    public Dictionary<GameObject, float> targetTanksInSight = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesInSight = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesInSight = new Dictionary<GameObject, float>();

    public Dictionary<GameObject, Dictionary<String, Vector3>> consumablesLastSeen = new Dictionary<GameObject, Dictionary<String, Vector3>>();
    [HideInInspector]
    public GameObject targetTankPrediction;
    [HideInInspector]
    public GameObject targetTankPosition;
    [HideInInspector]
    public GameObject consumablePosition;
    [HideInInspector]
    public GameObject basePosition;
    [HideInInspector]
    public Material tanktopcolor;
    [HideInInspector]
    public Material tankbodycolor;


    public BTAction healthCheck;
    public BTAction ammoCheck;
    public BTAction fuelCheck;
    public BTSequence battleReadyCheck;



    private StateMachine_TONKS_BT BT;

    private float fuelPanicLimit = 125f;
    private float fuelSurvivalLimit = 15f;
    private float hpPanicLimit = 125f;
    private int ammoPanicLimit = 15;

    private float rgb = 0f;



    private void Awake()
    {
        if(BT == null) {
            BT = gameObject.AddComponent<StateMachine_TONKS_BT>();
        }
    }
    private void InitializeStateMachine(){
        targetTankPrediction = new GameObject("TonksAimPosition");
        Dictionary<Type, BaseState_TONKS_BT> states = new Dictionary<Type, BaseState_TONKS_BT>
        {
            { typeof(ChoiceState_TONKS_BT), new ChoiceState_TONKS_BT() },

            { typeof(AttackTankState_TONKS_BT), new AttackTankState_TONKS_BT() },
            { typeof(AttackBaseState_TONKS_BT), new AttackBaseState_TONKS_BT() },
            { typeof(CollectState_TONKS_BT), new CollectState_TONKS_BT() },
            { typeof(WanderState_TONKS_BT), new WanderState_TONKS_BT() },
            { typeof(SurvivalState_TONKS_BT), new SurvivalState_TONKS_BT() }
        };
        BT.SetStates(states);
        BT.SetTank();
        targetTankPrediction.transform.SetParent(transform.parent.transform);
    }



    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart(){
        
        tankbodycolor = transform.Find("Model").Find("Body").GetComponent<Renderer>().material;
        tanktopcolor = transform.Find("Model").Find("Turret").Find("TurretPart").GetComponent<Renderer>().material;
        //basecolor = transform.Find("Base").Find("Model").GetComponent<Renderer>().material;
        InitializeStateMachine();
        healthCheck = new BTAction(HealthCheck);
        ammoCheck = new BTAction(AmmoCheck);
        fuelCheck = new BTAction(FuelCheck);
        battleReadyCheck = new BTSequence(new List<BTBaseNode> { healthCheck, ammoCheck, fuelCheck});



       
    }

    /*******************************************************************************************************       
    WARNING, do not include void Update(), use AITankUpdate() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankUpdate(){
        //Get the targets found from the sensor view
        targetTanksInSight = GetAllTargetTanksFound;
        consumablesInSight = GetAllConsumablesFound;
        basesInSight = GetAllBasesFound;
        InSightUpdate();
        RGBupdate();
    }

    public float GetHealth{ get{ return GetHealthLevel; }}
    public float GetAmmo{ get{ return GetAmmoLevel; }}
    public float GetFuel{ get { return GetFuelLevel; }}
    public float FuelPanicLimit { get { return fuelPanicLimit; }}
    public float FuelSurvivalLimit { get { return fuelSurvivalLimit; }}
    public float HPPanicLimit { get { return hpPanicLimit; }}
    public float AmmoPanicLimit { get { return ammoPanicLimit; }}

    public void PathToRandom(float x = 1f){ FollowPathToRandomPoint(x); }
    public void NewRandomPoint(){ GenerateRandomPoint(); }
    public void PathTo(GameObject point,float speed = 1f) { FollowPathToPoint(point, speed); }
    public void ShootAt(GameObject target){ FireAtPoint(target); }


    public void InSightUpdate(){
        //i'm only using a last seen position for these, not cheating to use current positions
       
        foreach(KeyValuePair<GameObject, float> item in consumablesInSight){
            if (!consumablesLastSeen.ContainsKey(item.Key)) {
                consumablesLastSeen.Add(item.Key, new Dictionary<String, Vector3> { { item.Key.name, item.Key.transform.position } });
            }
            else{
                consumablesLastSeen[item.Key] = new Dictionary<String, Vector3> { { item.Key.name, item.Key.transform.position } };
            }
        }
    }
    public void lookat(Vector3 point) {
        FaceTurretToPoint(point);
    }
    public void RGBupdate() {
        rgb += Time.fixedDeltaTime / 16f;
        rgb %= 1f;
        tankbodycolor.color = Color.HSVToRGB(rgb, 1f, 1f);
        tanktopcolor.color = Color.HSVToRGB(rgb , 1f, 1f);
    }
    public BTNodeStates HealthCheck() {
        if(GetHealthLevel >= 30f) {

            return BTNodeStates.FAILURE;
        }
        else {
            return BTNodeStates.SUCCESS;
        }
    }

    public BTNodeStates AmmoCheck() {
        if(GetAmmoLevel == 0) {

            return BTNodeStates.FAILURE;
        }
        else {
            return BTNodeStates.SUCCESS;
        }
    }
    public BTNodeStates FuelCheck() {
        if(GetFuelLevel <= 15f) {

            return BTNodeStates.FAILURE;
        }
        else {
            return BTNodeStates.SUCCESS;
        }
    }
    public BTNodeStates ItemSpottedCheck() {
        if(consumablesLastSeen.Count != 0 && consumablesLastSeen.First().Key != null) {
            return BTNodeStates.SUCCESS;
        }
        else {
            return BTNodeStates.FAILURE;
        }
    }

    public BTNodeStates TankSpottedCheck() {
        if(targetTanksInSight.Count != 0 && targetTanksInSight.First().Key != null) {
            return BTNodeStates.SUCCESS;
        }
        else {
            return BTNodeStates.FAILURE;
        }
    }
    public BTNodeStates BaseSpottedCheck() {
        if(basesInSight.Count != 0 && basesInSight.First().Key != null) {
            return BTNodeStates.SUCCESS;
        }
        else {
            return BTNodeStates.FAILURE;
        }
    }
    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision){ }
}
