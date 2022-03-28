using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_TONKS_RBS : MonoBehaviour
{

    private Dictionary<Type, BaseState_TONKS_RBS> states;

    public BaseState_TONKS_RBS currentState;
    protected SmartTank_TONKS_RBS me;
    public void SetTank(){
        me = GetComponent<SmartTank_TONKS_RBS>();
    }


    public BaseState_TONKS_RBS CurrentState{
        get{
            return currentState;
        }
        private set{
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState_TONKS_RBS> states){
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
