using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurvivalState_TONKS_RBS : BaseState_TONKS_RBS {
    // Start is called before the first frame update
    public SurvivalState_TONKS_RBS() { }

    public override Type StateEnter(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_RBS me) {
        if(!me.stats["lowFuel"]) {
            return typeof(ChoiceState_TONKS_RBS);
        }
        if(me.consumablesLastSeen.Count != 0) {
            foreach(KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen) {
                if(item.Value.First().Key == "Fuel") {
                    me.PathTo(item.Key);
                    if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                        me.consumablesLastSeen.Remove(item.Key);
                    }
                    return typeof(ChoiceState_TONKS_RBS);
                }
            }
        }
        else {
            me.PathTo(me.gameObject,0f);
        }
        return typeof(ChoiceState_TONKS_RBS);
    }
}
