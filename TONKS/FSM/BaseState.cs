using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    SmartTank me;
    public abstract Type StateUpdate(SmartTank me);
    public abstract Type StateEnter(SmartTank me);
    public abstract Type StateExit(SmartTank me);
}
