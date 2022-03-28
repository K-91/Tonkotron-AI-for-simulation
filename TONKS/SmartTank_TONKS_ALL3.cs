using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class SmartTank_TONKS_ALL3 : AITank
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


    public BTAction3 healthCheck;
    public BTAction3 healthMaxCheck;
    public BTAction3 ammoCheck;
    public BTAction3 fuelCheck;
    public BTAction3 tankSpottedCheck;
    public BTAction3 itemSpottedCheck;
    public BTAction3 baseSpottedCheck;
    public BTSequence3 battleReadyCheck;





    public Rules3 rules3 = new Rules3();
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();

    private StateMachine_TONKS_ALL3 ALL3;

    private float rgb = 0f;



    private void Awake()
    {
        if(ALL3 == null) {
            ALL3 = gameObject.AddComponent<StateMachine_TONKS_ALL3>();
        }

    }
    private void InitializeStateMachine(){
        targetTankPrediction = new GameObject("TonksAimPosition");
        Dictionary<Type, BaseState_TONKS_ALL3> states = new Dictionary<Type, BaseState_TONKS_ALL3>
        {
            { typeof(ChoiceState_TONKS_ALL3), new ChoiceState_TONKS_ALL3() },

            { typeof(AttackTankState_TONKS_ALL3), new AttackTankState_TONKS_ALL3() },
            { typeof(AttackBaseState_TONKS_ALL3), new AttackBaseState_TONKS_ALL3() },
            { typeof(CollectState_TONKS_ALL3), new CollectState_TONKS_ALL3() },
            { typeof(WanderState_TONKS_ALL3), new WanderState_TONKS_ALL3() },
            { typeof(SurvivalState_TONKS_ALL3), new SurvivalState_TONKS_ALL3() }
        };
        ALL3.SetStates(states);
        ALL3.SetTank();
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

        stats.Add("lowHP", false);
        stats.Add("maxHP", false);
        stats.Add("tankSpotted", false);
        stats.Add("baseSpotted", false);
        stats.Add("battleReady", false);
        stats.Add("noAmmo", false);
        stats.Add("lowFuel", false);
        stats.Add("foundItem", false);
        stats.Add("true", true);
        stats.Add("false", false);

        rules3.AddRule(new Rule3("foundItem","true",typeof(CollectState_TONKS_ALL3),Rule3.Predicate.And));
        rules3.AddRule(new Rule3("tankSpotted","battleReady",typeof(AttackTankState_TONKS_ALL3),Rule3.Predicate.And));
        rules3.AddRule(new Rule3("lowFuel", "tankSpotted" ,typeof(AttackTankState_TONKS_ALL3),Rule3.Predicate.And));

        rules3.AddRule(new Rule3("baseSpotted", "battleReady" ,typeof(AttackBaseState_TONKS_ALL3),Rule3.Predicate.And));
        rules3.AddRule(new Rule3("lowFuel", "true" ,typeof(SurvivalState_TONKS_ALL3),Rule3.Predicate.And));



        healthCheck = new BTAction3(HealthCheck);
        healthMaxCheck = new BTAction3(HealthMaxCheck);
        ammoCheck = new BTAction3(AmmoCheck);
        fuelCheck = new BTAction3(FuelCheck);
        itemSpottedCheck = new BTAction3(ItemSpottedCheck);
        tankSpottedCheck = new BTAction3(TankSpottedCheck);
        baseSpottedCheck = new BTAction3(BaseSpottedCheck);
        battleReadyCheck = new BTSequence3(new List<BTBaseNode3> { healthCheck, ammoCheck, fuelCheck });


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
        StatsUpdate();
        RGBupdate();
    }

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
    public void StatsUpdate() {
        stats["lowHP"] = (GetHealthLevel <= 30f);
        stats["maxHP"] = (GetHealthLevel >= 125f);
        stats["tankSpotted"] = (targetTanksInSight.Count != 0 && targetTanksInSight.First().Key != null);
        stats["baseSpotted"] = (basesInSight.Count != 0 && basesInSight.First().Key != null);
        stats["foundItem"] = (consumablesLastSeen.Count != 0 && consumablesLastSeen.First().Key != null);
        stats["noAmmo"] = (GetAmmoLevel == 0);
        stats["lowFuel"] = (GetFuelLevel <= 15f);
        stats["battleReady"] = (battleReadyCheck.Evaluate() == BTNodeStates3.SUCCESS) ? true : false ;
    }
    public BTNodeStates3 HealthCheck() {
        if(stats["lowHP"]) {

            return BTNodeStates3.FAILURE;
        }
        else {
            return BTNodeStates3.SUCCESS;
        }
    }
    public BTNodeStates3 HealthMaxCheck() {
        if(stats["maxHP"]) {

            return BTNodeStates3.SUCCESS;
        }
        else {
            return BTNodeStates3.FAILURE;
        }
    }

    public BTNodeStates3 AmmoCheck() {
        if(stats["noAmmo"]) {

            return BTNodeStates3.FAILURE;
        }
        else {
            return BTNodeStates3.SUCCESS;
        }
    }
    public BTNodeStates3 FuelCheck() {
        if(stats["lowFuel"]) {

            return BTNodeStates3.FAILURE;
        }
        else {
            return BTNodeStates3.SUCCESS;
        }
    }
    public BTNodeStates3 ItemSpottedCheck() {
        if(stats["foundItem"]) {
            return BTNodeStates3.SUCCESS;
        }
        else {
            return BTNodeStates3.FAILURE;
        }
    }

    public BTNodeStates3 TankSpottedCheck() {
        if(stats["tankSpotted"]) {
            return BTNodeStates3.SUCCESS;
        }
        else {
            return BTNodeStates3.FAILURE;
        }
    }
    public BTNodeStates3 BaseSpottedCheck() {
        if(stats["baseSpotted"]) {
            return BTNodeStates3.SUCCESS;
        }
        else {
            return BTNodeStates3.FAILURE;
        }
    }
    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision){ }
}
