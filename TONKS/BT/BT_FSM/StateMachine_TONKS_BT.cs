using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_TONKS_BT : MonoBehaviour
{

    private Dictionary<Type, BaseState_TONKS_BT> states;

    public BaseState_TONKS_BT currentState;
    protected SmartTank_TONKS_BT me;
    public void SetTank(){
        me = GetComponent<SmartTank_TONKS_BT>();
    }


    public BaseState_TONKS_BT CurrentState {
        get{
            return currentState;
        }
        private set{
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState_TONKS_BT> states){
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
