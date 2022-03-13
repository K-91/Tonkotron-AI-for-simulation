using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceState : BaseState
{
    // Start is called before the first frame update
    public ChoiceState(){}

    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }

    public override Type StateUpdate(SmartTank me){
        
        //if we can see another tank and we're healthy we should fight
        if (me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null && me.GetHealth > 30f){
            return typeof(AttackTankState);
        }
        //if there's not much to do, we're not full but we know where something is, let's go and get it
        foreach (KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen)
        {
            if (item.Value.ContainsKey("Fuel")){
                return typeof(CollectState);
            }
            else if (me.GetHealth <= me.HPPanicLimit && item.Value.ContainsKey("Health") && me.GetFuel > me.FuelSurvivalLimit){
                return typeof(CollectState);
            }
            else if (item.Value.ContainsKey("Ammo") && me.GetFuel > me.FuelSurvivalLimit){
                return typeof(CollectState);
            }
            
        }
      
        //if we know where the enemy base is, we can go and shoot it
        if (me.basesInSight.Count != 0 && me.GetFuel > me.FuelSurvivalLimit){
            return typeof(AttackBaseState);
        }
        //if we're dangerously low on fuel, we'll stay still in hopes the other guy runs out first
        if(me.GetFuel <= me.FuelSurvivalLimit) {
            return typeof(SurvivalState);
        }

        //if we don't know what to do, we'll just look for things
        return typeof(WanderState);
    }
}
