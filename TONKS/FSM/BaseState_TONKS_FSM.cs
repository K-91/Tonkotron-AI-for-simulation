using System;


public abstract class BaseState_TONKS_FSM 
{
    SmartTank_TONKS_FSM me;
    public abstract Type StateUpdate(SmartTank_TONKS_FSM me);
    public abstract Type StateEnter(SmartTank_TONKS_FSM me);
    public abstract Type StateExit(SmartTank_TONKS_FSM me);
}
