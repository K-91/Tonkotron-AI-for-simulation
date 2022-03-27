using System;
using UnityEngine;

public class WanderState_TONKS_FSM : BaseState_TONKS_FSM
{
    float t = 0;
    // Start is called before the first frame update
    public WanderState_TONKS_FSM() { }

    public override Type StateEnter(SmartTank_TONKS_FSM me) {
        return null;
    }

    public override Type StateExit(SmartTank_TONKS_FSM me) {
        return null;
    }

    public override Type StateUpdate(SmartTank_TONKS_FSM me){
        t += Time.deltaTime;
        if(t > 10f){
            me.NewRandomPoint();
            t = 0f;
        }
        //can't risk going slow at low hp
        if(me.GetHealth >= 30f) {
            me.PathToRandom(1f);
        }
        //put we will go a lil slower if we can to hope the other guy runs out of fuel
        else {
            me.PathToRandom(0.95f);
        }
        return typeof(ChoiceState_TONKS_FSM);
    }
}
