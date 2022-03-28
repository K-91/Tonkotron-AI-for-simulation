using System;


public abstract class BaseState_TONKS_RBS 
{
    SmartTank_TONKS_RBS me;
    public abstract Type StateUpdate(SmartTank_TONKS_RBS me);
    public abstract Type StateEnter(SmartTank_TONKS_RBS me);
    public abstract Type StateExit(SmartTank_TONKS_RBS me);
}
