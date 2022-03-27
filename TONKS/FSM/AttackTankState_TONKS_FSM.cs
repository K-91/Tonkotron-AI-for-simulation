using System;
using System.Linq;
using UnityEngine;

public class AttackTankState_TONKS_FSM : BaseState_TONKS_FSM
{
    // Start is called before the first frame update
    public AttackTankState_TONKS_FSM() { }
    Vector3 enemyLastFramePos;
    Vector3 enemyThisFramePos;
    Vector3 predictedDirection;
    public override Type StateEnter(SmartTank_TONKS_FSM me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_FSM me) {
        return null;
    }
    public override Type StateUpdate(SmartTank_TONKS_FSM me){
        if (me.targetTanksInSight.Count > 0 && me.targetTanksInSight.First().Key != null)
        {
            //predict moving targets
            me.targetTankPosition = me.targetTanksInSight.First().Key;
            enemyThisFramePos = me.targetTanksInSight.First().Key.transform.position;
            predictedDirection = (enemyThisFramePos - enemyLastFramePos);
            if((enemyLastFramePos - enemyThisFramePos).magnitude > 0.1f) {// if moving use movement prediction
                me.targetTankPrediction.transform.position = enemyThisFramePos + (predictedDirection.normalized * 30f);
            }
            else {//else use basic position
                me.targetTankPrediction.transform.position = enemyThisFramePos;
            }
            //look down a bit if we're too close (did you know you can take damage from your own shot? don't look down further than 0.5f)
            Vector3 temp = me.targetTankPrediction.transform.position;
            if(Vector3.Distance(me.targetTankPrediction.transform.position, me.transform.position) < 10f) {
 
                temp.y -= 0.5f;

            }
            else {
                temp.y += 0.1f;
            }
            me.targetTankPrediction.transform.position = temp;
            if (me.targetTankPosition != null)
            {
                
                //get closer to target, and fire
                if (Vector3.Distance(me.transform.position, me.targetTankPosition.transform.position) < 8f) {
                    me.ShootAt(me.targetTankPrediction);
                }//if not close enough then let's go towards them
                else if(me.GetFuel > me.FuelSurvivalLimit) {
                    me.PathTo(me.targetTankPrediction);
                }
            }


            enemyLastFramePos = me.targetTanksInSight.First().Key.transform.position;
        }
        return typeof(ChoiceState_TONKS_FSM);
    }
}
