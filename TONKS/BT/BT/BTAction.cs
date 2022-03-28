using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAction : BTBaseNode
{
    //Stores the function signature for the action
    public delegate BTNodeStates ActionNodeFunction();

    //Called to evaluate this node
    private ActionNodeFunction btAction;

    //The function is passed in and stored upon creating the action node
    public BTAction(ActionNodeFunction btAction)
    {
        this.btAction = btAction;
    }

    // Evaluates the actio node
    public override BTNodeStates Evaluate()
    {
        switch (btAction())
        {
            case BTNodeStates.SUCCESS:
                btNodeState = BTNodeStates.SUCCESS;
                return btNodeState;
            case BTNodeStates.FAILURE:
                btNodeState = BTNodeStates.FAILURE;
                return btNodeState;
            default:
                btNodeState = BTNodeStates.FAILURE;
                return btNodeState;
        }
    }
}

