using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceState_TONKS_BT : BaseState_TONKS_BT {
    // Start is called before the first frame update
    public ChoiceState_TONKS_BT(){}

    public override Type StateEnter(SmartTank_TONKS_BT me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_BT me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_BT me){

        //keep enemy in sights if we can
        if(me.tankSpottedCheck.Evaluate() == BTNodeStates.SUCCESS) {
            try {
                me.lookat(me.targetTankPosition.transform.position);
            }
            catch(UnassignedReferenceException) { }
        }
        else {
            //keep aim ahead if we can't see them
            me.targetTankPrediction.transform.position = me.transform.position + (me.transform.forward*10);
        }

        //if there's not much to do, we're not full but we know where something is, let's go and get it
            foreach (KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen)
        {
            if (item.Value.ContainsKey("Fuel")){
                return typeof(CollectState_TONKS_BT);
            }
            else if (me.healthMaxCheck.Evaluate() == BTNodeStates.SUCCESS && item.Value.ContainsKey("Health") && me.fuelCheck.Evaluate() == BTNodeStates.SUCCESS) {
                return typeof(CollectState_TONKS_BT);
            }
            else if (item.Value.ContainsKey("Ammo") && me.fuelCheck.Evaluate() == BTNodeStates.SUCCESS) {
                return typeof(CollectState_TONKS_BT);
            }
            
        }
        //if we can see another tank and we're healthy we should fight
        if (me.tankSpottedCheck.Evaluate() == BTNodeStates.SUCCESS && me.healthCheck.Evaluate() == BTNodeStates.SUCCESS) {
            return typeof(AttackTankState_TONKS_BT);
        }
        //if we can see another tank and are stuck, we may as well fight
        if(me.tankSpottedCheck.Evaluate() == BTNodeStates.SUCCESS && me.fuelCheck.Evaluate() == BTNodeStates.FAILURE) {
            return typeof(AttackTankState_TONKS_BT);
        }

        //if we know where the enemy base is, we can shoot it
        if (me.baseSpottedCheck.Evaluate() == BTNodeStates.SUCCESS && me.fuelCheck.Evaluate() == BTNodeStates.SUCCESS) {
            return typeof(AttackBaseState_TONKS_BT);
        }
        //if we're dangerously low on fuel, we'll stay still in hopes the other guy runs out first
        if(me.fuelCheck.Evaluate() == BTNodeStates.FAILURE) {
            return typeof(SurvivalState_TONKS_BT);
        }

        //if we don't know what to do, we'll just look for things
        return typeof(WanderState_TONKS_BT);
    }
}
