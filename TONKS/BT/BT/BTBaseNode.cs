using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTBaseNode
{
    //The current state of the node
    protected BTNodeStates btNodeState;

    //Return node state
    public BTNodeStates BTNodeState
    {
        get { return btNodeState; }
    }

    //Evaluate the desired set of conditions 
    public abstract BTNodeStates Evaluate();

}

