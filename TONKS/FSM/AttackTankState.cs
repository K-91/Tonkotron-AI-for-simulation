using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackTankState : BaseState
{
    // Start is called before the first frame update
    public AttackTankState() { }
    Vector3 enemyLastFramePos;
    Vector3 enemyThisFramePos;
    Vector3 predictedDirection;
    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }
    public override Type StateUpdate(SmartTank me){
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

            if (me.targetTankPosition != null)
            {
                
                //get closer to target, and fire
                if (Vector3.Distance(me.transform.position, me.targetTankPosition.transform.position) < 40f) {
                    me.ShootAt(me.targetTankPrediction);
                }//if not close enough then let's go towards them
                else if(me.GetFuel > me.FuelSurvivalLimit) {
                    me.PathTo(me.targetTankPrediction);
                }
            }


            enemyLastFramePos = me.targetTanksInSight.First().Key.transform.position;
        }
        return typeof(ChoiceState);
    }
}
