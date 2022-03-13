using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : BaseState
{
    float t = 0;
    // Start is called before the first frame update
    public WanderState() { }

    public override Type StateEnter(SmartTank me) {
        return null;
    }

    public override Type StateExit(SmartTank me) {
        return null;
    }

    public override Type StateUpdate(SmartTank me){
        t += Time.deltaTime;
        if(t > 10f){
            me.NewRandomPoint();
            t = 0f;
        }
        me.PathToRandom(1f);
        return typeof(ChoiceState);
    }
}
