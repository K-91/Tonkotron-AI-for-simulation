using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurvivalState : BaseState {
    // Start is called before the first frame update
    public SurvivalState() { }

    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }

    public override Type StateUpdate(SmartTank me) {
        me.targetTankPrediction.transform.position = me.transform.position;
        me.PathTo(me.targetTankPrediction,0f);
        if(me.consumablesLastSeen.Count != 0) {
            foreach(KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen) {
                if(item.Value.First().Key == "Fuel") {
                    me.PathTo(item.Key);
                    return typeof(ChoiceState);
                }
            }
        }
        return typeof(ChoiceState);
    }
}
