using System;


public abstract class BaseState_TONKS_ALL3 {
    SmartTank_TONKS_ALL3 me;
    public abstract Type StateUpdate(SmartTank_TONKS_ALL3 me);
    public abstract Type StateEnter(SmartTank_TONKS_ALL3 me);
    public abstract Type StateExit(SmartTank_TONKS_ALL3 me);
}
