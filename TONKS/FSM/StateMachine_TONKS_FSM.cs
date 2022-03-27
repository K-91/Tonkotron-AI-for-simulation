using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_TONKS_FSM : MonoBehaviour
{

    private Dictionary<Type, BaseState_TONKS_FSM> states;

    public BaseState_TONKS_FSM currentState;
    protected SmartTank_TONKS_FSM me;
    public void SetTank(){
        me = GetComponent<SmartTank_TONKS_FSM>();
    }


    public BaseState_TONKS_FSM CurrentState{
        get{
            return currentState;
        }
        private set{
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState_TONKS_FSM> states){
        this.states = states;
    }

    void Update(){
        if(CurrentState == null){
            try{
                CurrentState = states.Values.First();
            }
            catch(NullReferenceException){}
        }
        else{
            var nextState = CurrentState.StateUpdate(me);

            if(nextState != null && nextState != CurrentState.GetType()){
                SwitchToState(nextState);
            }
        }
    }

    void SwitchToState(Type nextState){
        CurrentState.StateExit(me);
        CurrentState = states[nextState];
        CurrentState.StateEnter(me);
    }
}
