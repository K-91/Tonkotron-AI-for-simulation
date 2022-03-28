using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectState_TONKS_ALL3 : BaseState_TONKS_ALL3 {
    // Start is called before the first frame update
    public CollectState_TONKS_ALL3() { }

    public override Type StateEnter(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_ALL3 me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_ALL3 me) {
        
        
        try {
            foreach(KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen) {
                try {
                    if(item.Value.First().Key == "Fuel") {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                            me.consumablesLastSeen.Remove(item.Key);
                        }
                        return typeof(ChoiceState_TONKS_ALL3);
                    }
                    else if(me.healthMaxCheck.Evaluate() == BTNodeStates3.FAILURE && item.Value.First().Key == "Health" && me.fuelCheck.Evaluate() == BTNodeStates3.SUCCESS) {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                            me.consumablesLastSeen.Remove(item.Key);
                        }
                        return typeof(ChoiceState_TONKS_ALL3);
                    }
                    else if(item.Value.First().Key == "Ammo" && me.fuelCheck.Evaluate() == BTNodeStates3.SUCCESS) {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        
                        if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                            me.consumablesLastSeen.Remove(item.Key);
                        }
                        return typeof(ChoiceState_TONKS_ALL3);
                    }
                    if(Vector3.Distance(me.transform.position, me.consumablePosition.transform.position) < 4f) {
                        me.consumablesLastSeen.Remove(item.Key);
                    }
                }
                catch(UnassignedReferenceException) {
                    me.consumablesLastSeen.Remove(item.Key);
                }
                catch(MissingReferenceException) {
                    me.consumablesLastSeen.Remove(item.Key);
                }
                
            }
            //if we're really low, we'll kick into survival state where we stay still to not die to fuel costs
            if(me.fuelCheck.Evaluate() == BTNodeStates3.FAILURE) {
            return typeof(SurvivalState_TONKS_ALL3);
            }
        }
        catch(InvalidOperationException) { }
        return typeof(ChoiceState_TONKS_ALL3);
    }
}
