using System;


public abstract class BaseState_TONKS_BT {
    SmartTank_TONKS_BT me;
    public abstract Type StateUpdate(SmartTank_TONKS_BT me);
    public abstract Type StateEnter(SmartTank_TONKS_BT me);
    public abstract Type StateExit(SmartTank_TONKS_BT me);
}
