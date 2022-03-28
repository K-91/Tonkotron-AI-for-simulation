using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceState_TONKS_RBS : BaseState_TONKS_RBS
{
    // Start is called before the first frame update
    public ChoiceState_TONKS_RBS(){}

    public override Type StateEnter(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_RBS me){

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

        foreach(var item in me.rules.GetRules) {
            if(item.CheckRule(me.stats) != null) {
                return item.CheckRule(me.stats);
            }

        }
        //if we don't know what to do, we'll just look for things
        return typeof(WanderState_TONKS_RBS);
    }
}
