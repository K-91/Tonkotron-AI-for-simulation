using System;
using System.Linq;
using UnityEngine;

public class AttackTankState_TONKS_BT : BaseState_TONKS_BT {
    // Start is called before the first frame update
    public AttackTankState_TONKS_BT() { }
    Vector3 enemyLastFramePos;
    Vector3 enemyThisFramePos;
    Vector3 predictedDirection;
    public override Type StateEnter(SmartTank_TONKS_BT me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_BT me) {
        return null;
    }
    public override Type StateUpdate(SmartTank_TONKS_BT me){
        if (me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null)
        {
            //predict moving targets
            me.targetTankPosition = me.targetTanksInSight.First().Key;
            enemyThisFramePos = me.targetTanksInSight.First().Key.transform.position;
            predictedDirection = (enemyThisFramePos - enemyLastFramePos);
            if((enemyLastFramePos - enemyThisFramePos).magnitude > 0.1f && Vector3.Distance(me.targetTankPrediction.transform.position, me.transform.position) > 9f) {// if moving use movement prediction
                me.targetTankPrediction.transform.position = enemyThisFramePos + (predictedDirection.normalized * 15f);
            }
            else {//else use basic position
                me.targetTankPrediction.transform.position = enemyThisFramePos;
            }
            //look down a bit if we're too close (did you know you can take damage from your own shot? don't look down further than 0.4f)
            Vector3 temp = me.targetTankPrediction.transform.position;
            if(Vector3.Distance(me.targetTankPrediction.transform.position, me.transform.position) < 10f) {
 
                temp.y -= 0.4f;

            }
            else {
                temp.y += 0.075f;
            }
            me.targetTankPrediction.transform.position = temp;
            if (me.targetTankPosition != null)
            {
                
                //get closer to target, and fire
                if (Vector3.Distance(me.transform.position, me.targetTankPosition.transform.position) < 9f) {
                    me.ShootAt(me.targetTankPrediction);
                }//if not close enough then let's go towards them
                else{
                    me.PathTo(me.targetTankPosition);
                }
            }
            enemyLastFramePos = me.targetTanksInSight.First().Key.transform.position;
        }
        return typeof(ChoiceState_TONKS_BT);
    }
}
