using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurvivalState_TONKS_ALL3 : BaseState_TONKS_ALL3 {
    // Start is called before the first frame update
    public SurvivalState_TONKS_ALL3() { }

    public override Type StateEnter(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_ALL3 me) {
        if(me.fuelCheck.Evaluate() == BTNodeStates3.FAILURE) {
            return typeof(ChoiceState_TONKS_ALL3);
        }
        Debug.Log("eval item");
        if(me.itemSpottedCheck.Evaluate() == BTNodeStates3.SUCCESS) {
            foreach(KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen) {
                if(item.Value.First().Key == "Fuel") {
                    me.PathTo(item.Key);
                    if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                        me.consumablesLastSeen.Remove(item.Key);
                    }
                    return typeof(ChoiceState_TONKS_ALL3);
                }
            }
        }
        else {
            me.PathTo(me.gameObject,0f);
        }
        return typeof(ChoiceState_TONKS_ALL3);
    }
}
