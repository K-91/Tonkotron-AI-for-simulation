using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackBaseState : BaseState
{
    // Start is called before the first frame update
    public AttackBaseState() { }

    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }

    public override Type StateUpdate(SmartTank me){
        if(me.basesInSight.Count != 0) {
            me.basePosition = me.basesInSight.First().Key;
            try {
                if(Vector3.Distance(me.transform.position, me.basePosition.transform.position) < 25f) {
                    me.ShootAt(me.basePosition);
                }
                else {
                    me.PathTo(me.basePosition);
                }
            }
            catch(UnassignedReferenceException) { }
            catch(MissingReferenceException) { }
        }
        return typeof(ChoiceState);
    }
}
