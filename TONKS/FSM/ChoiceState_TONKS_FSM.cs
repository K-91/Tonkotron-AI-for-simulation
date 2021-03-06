using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceState_TONKS_FSM : BaseState_TONKS_FSM
{
    // Start is called before the first frame update
    public ChoiceState_TONKS_FSM(){}

    public override Type StateEnter(SmartTank_TONKS_FSM me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_FSM me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_FSM me){

        //keep enemy in sights if we can
        if(me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null) {
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
                return typeof(CollectState_TONKS_FSM);
            }
            else if (me.GetHealth <= me.HPPanicLimit && item.Value.ContainsKey("Health") && me.GetFuel > me.FuelSurvivalLimit){
                return typeof(CollectState_TONKS_FSM);
            }
            else if (item.Value.ContainsKey("Ammo") && me.GetFuel > me.FuelSurvivalLimit){
                return typeof(CollectState_TONKS_FSM);
            }
            
        }
        //if we can see another tank and we're healthy we should fight
        if (me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null && me.GetHealth > 30f) {
            return typeof(AttackTankState_TONKS_FSM);
        }
        //if we can see another tank and are stuck, we may as well fight
        if(me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null && (me.GetFuel < me.FuelSurvivalLimit)) {
            return typeof(AttackTankState_TONKS_FSM);
        }

        //if we know where the enemy base is, we can shoot it
        if (me.basesInSight.Count != 0 && me.GetFuel > me.FuelSurvivalLimit){
            return typeof(AttackBaseState_TONKS_FSM);
        }
        //if we're dangerously low on fuel, we'll stay still in hopes the other guy runs out first
        if(me.GetFuel <= me.FuelSurvivalLimit) {
            return typeof(SurvivalState_TONKS_FSM);
        }

        //if we don't know what to do, we'll just look for things
        return typeof(WanderState_TONKS_FSM);
    }
}
