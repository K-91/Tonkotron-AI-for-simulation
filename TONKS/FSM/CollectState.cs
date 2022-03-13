using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectState : BaseState {
    // Start is called before the first frame update
    public CollectState() { }

    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }

    public override Type StateUpdate(SmartTank me) {
        
        
        try {
            foreach(KeyValuePair<GameObject, Dictionary<String, Vector3>> item in me.consumablesLastSeen) {
                try {

                    //TODO: clear pathfinding of an item once collected

                    if(item.Value.First().Key == "Fuel") {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        return typeof(ChoiceState);
                    }
                    else if(me.GetHealth < me.HPPanicLimit && item.Value.First().Key == "Health" && me.GetFuel > me.FuelSurvivalLimit) {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        return typeof(ChoiceState);
                    }
                    else if(item.Value.First().Key == "Ammo" && me.GetFuel > me.FuelSurvivalLimit) {
                        me.consumablePosition = item.Key;
                        me.PathTo(me.consumablePosition);
                        return typeof(ChoiceState);
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
            if(me.GetFuel <= me.FuelSurvivalLimit) {
            return typeof(SurvivalState);
            }
        }
        catch(InvalidOperationException) { }
        return typeof(ChoiceState);
    }
}
