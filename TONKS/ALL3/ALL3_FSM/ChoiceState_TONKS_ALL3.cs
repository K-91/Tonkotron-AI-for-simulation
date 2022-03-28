using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceState_TONKS_ALL3 : BaseState_TONKS_ALL3 {
    // Start is called before the first frame update
    public ChoiceState_TONKS_ALL3(){}

    public override Type StateEnter(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_ALL3 me){

        //keep enemy in sights if we can
        if(me.tankSpottedCheck.Evaluate() == BTNodeStates3.SUCCESS) {
            try {
                me.lookat(me.targetTankPosition.transform.position);
            }
            catch(UnassignedReferenceException) { }
        }
        else {
            //keep aim ahead if we can't see them
            me.targetTankPrediction.transform.position = me.transform.position + (me.transform.forward*10);
        }

        foreach(var item in me.rules3.GetRules) {
            if(item.CheckRule(me.stats) != null) {
                return item.CheckRule(me.stats);
            }

        }
        //if we don't know what to do, we'll just look for things
        return typeof(WanderState_TONKS_ALL3);
    }
}
