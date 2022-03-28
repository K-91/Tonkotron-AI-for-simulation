using System;
using System.Linq;
using UnityEngine;

public class AttackBaseState_TONKS_RBS : BaseState_TONKS_RBS
{
    // Start is called before the first frame update
    public AttackBaseState_TONKS_RBS() { }

    public override Type StateEnter(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_RBS me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_RBS me){
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
        return typeof(ChoiceState_TONKS_RBS);
    }
}
