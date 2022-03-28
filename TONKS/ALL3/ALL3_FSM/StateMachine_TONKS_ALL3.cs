using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_TONKS_ALL3 : MonoBehaviour
{

    private Dictionary<Type, BaseState_TONKS_ALL3> states;

    public BaseState_TONKS_ALL3 currentState;
    protected SmartTank_TONKS_ALL3 me;
    public void SetTank(){
        me = GetComponent<SmartTank_TONKS_ALL3>();
    }


    public BaseState_TONKS_ALL3 CurrentState {
        get{
            return currentState;
        }
        private set{
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState_TONKS_ALL3> states){
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
