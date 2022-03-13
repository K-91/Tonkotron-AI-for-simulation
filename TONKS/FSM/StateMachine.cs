using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    private Dictionary<Type, BaseState> states;

    public BaseState currentState;
    protected SmartTank me;
    public void SetTank(){
        me = GetComponent<SmartTank>();
    }


    public BaseState CurrentState{
        get{
            return currentState;
        }
        private set{
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState> states){
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
