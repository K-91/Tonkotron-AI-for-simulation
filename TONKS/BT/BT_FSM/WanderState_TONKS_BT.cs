using System;
using UnityEngine;

public class WanderState_TONKS_BT : BaseState_TONKS_BT {
    float t = 0;
    // Start is called before the first frame update
    public WanderState_TONKS_BT() { }

    public override Type StateEnter(SmartTank_TONKS_BT me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_BT me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_BT me){
        t += Time.deltaTime;
        if(t > 10f){
            me.NewRandomPoint();
            t = 0f;
        }
        //can't risk going slow at low hp
        if(me.GetHealth >= 30f) {
            me.PathToRandom();
        }
        //but we will go a lil slower if we can to hope the other guy runs out of fuel first
        else {
            me.PathToRandom(0.95f);
        }
        return typeof(ChoiceState_TONKS_BT);
    }
}
