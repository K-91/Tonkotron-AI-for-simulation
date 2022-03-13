using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SmartTank : AITank
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

    private StateMachine FSM;

    private float fuelPanicLimit = 125f;
    private float fuelSurvivalLimit = 15f;
    private float hpPanicLimit = 125f;
    private int ammoPanicLimit = 15;



    private void Awake()
    {
        FSM = GetComponent<StateMachine>();
    }
    private void InitializeStateMachine(){
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>
        {
            { typeof(ChoiceState), new ChoiceState() },

            { typeof(AttackTankState), new AttackTankState() },
            { typeof(AttackBaseState), new AttackBaseState() },
            { typeof(CollectState), new CollectState() },
            { typeof(WanderState), new WanderState() },
            { typeof(SurvivalState), new SurvivalState() }
        };
        FSM.SetStates(states);
        FSM.SetTank();
        targetTankPrediction = new GameObject("AimPosition");
    }




    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart(){
        InitializeStateMachine();
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
    }

    public float GetHealth{ get{ return GetHealthLevel; }}
    public float GetAmmo{ get{ return GetAmmoLevel; }}
    public float GetFuel{ get { return GetFuelLevel; }}
    public float FuelPanicLimit { get { return fuelPanicLimit; }}
    public float FuelSurvivalLimit { get { return fuelSurvivalLimit; }}
    public float HPPanicLimit { get { return hpPanicLimit; }}
    public float AmmoPanicLimit { get { return ammoPanicLimit; }}

    public void PathToRandom(float x){ FollowPathToRandomPoint(x); }
    public void NewRandomPoint(){ GenerateRandomPoint(); }
    public void PathTo(GameObject point,float speed = 1f) { FollowPathToPoint(point, speed); }
    public void ShootAt(GameObject target){ FireAtPoint(target); }


    public void InSightUpdate(){
        //i'm only storing/using a last seen position for these, not cheating to use enemy current positions
       
        foreach(KeyValuePair<GameObject, float> item in consumablesInSight){
            if (!consumablesLastSeen.ContainsKey(item.Key)) {
                consumablesLastSeen.Add(item.Key, new Dictionary<String, Vector3> { { item.Key.name, item.Key.transform.position } });
            }
            else{
                consumablesLastSeen[item.Key] = new Dictionary<String, Vector3> { { item.Key.name, item.Key.transform.position } };
            }
        }
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision){ }
}
