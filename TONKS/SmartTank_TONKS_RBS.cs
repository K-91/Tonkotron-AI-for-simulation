using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class SmartTank_TONKS_RBS : AITank
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

    public Rules rules = new Rules();
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();

    private StateMachine_TONKS_RBS RBS;

    private float rgb = 0f;



    private void Awake()
    {
        if(RBS == null) {
            RBS = gameObject.AddComponent<StateMachine_TONKS_RBS>();
        }

    }
    private void InitializeStateMachine(){
        targetTankPrediction = new GameObject("TonksAimPosition");
        Dictionary<Type, BaseState_TONKS_RBS> states = new Dictionary<Type, BaseState_TONKS_RBS>
        {
            { typeof(ChoiceState_TONKS_RBS), new ChoiceState_TONKS_RBS() },

            { typeof(AttackTankState_TONKS_RBS), new AttackTankState_TONKS_RBS() },
            { typeof(AttackBaseState_TONKS_RBS), new AttackBaseState_TONKS_RBS() },
            { typeof(CollectState_TONKS_RBS), new CollectState_TONKS_RBS() },
            { typeof(WanderState_TONKS_RBS), new WanderState_TONKS_RBS() },
            { typeof(SurvivalState_TONKS_RBS), new SurvivalState_TONKS_RBS() }
        };
        RBS.SetStates(states);
        RBS.SetTank();
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
        stats.Add("tankSpotted", false);
        stats.Add("baseSpotted", false);
        stats.Add("noAmmo", false);
        stats.Add("lowFuel", false);
        stats.Add("foundItem", false);
        stats.Add("battleReady", false);
        stats.Add("true", true);
        stats.Add("false", false);

        rules.AddRule(new Rule("foundItem","true",typeof(CollectState_TONKS_RBS),Rule.Predicate.And));
        rules.AddRule(new Rule("tankSpotted","battleReady",typeof(AttackTankState_TONKS_RBS),Rule.Predicate.And));
        rules.AddRule(new Rule("lowFuel", "tankSpotted" ,typeof(AttackTankState_TONKS_RBS),Rule.Predicate.And));

        rules.AddRule(new Rule("baseSpotted", "battleReady" ,typeof(AttackBaseState_TONKS_RBS),Rule.Predicate.And));
        rules.AddRule(new Rule("lowFuel", "true" ,typeof(SurvivalState_TONKS_RBS),Rule.Predicate.And));
        


       
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
        stats["tankSpotted"] = (targetTanksInSight.Count != 0 && targetTanksInSight.First().Key != null);
        stats["baseSpotted"] = (basesInSight.Count != 0 && basesInSight.First().Key != null);
        stats["foundItem"] = (consumablesLastSeen.Count != 0 && consumablesLastSeen.First().Key != null);
        stats["noAmmo"] = (GetAmmoLevel == 0);
        stats["lowFuel"] = (GetFuelLevel <= 15f);
        stats["battleReady"] = ((!stats["noAmmo"]) && (!stats["lowHP"]) && (!stats["lowFuel"]));
    }
    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision){ }
}
