using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTBaseNode
{
    //The children of the sequencer
    protected List<BTBaseNode> btNodes = new List<BTBaseNode>();

    //Children set through constructor
    public BTSequence(List<BTBaseNode> btNodes)
    {
        this.btNodes = btNodes;
    }

    //If any child node returns a failure, the entire node fails. 
    public override BTNodeStates Evaluate()
    {
        bool failed = false;
        foreach (BTBaseNode btNode in btNodes)
        {
            if (failed == true)
            {
                break;
            }

            switch (btNode.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    btNodeState = BTNodeStates.FAILURE;
                    failed = true;
                    break;
                case BTNodeStates.SUCCESS:
                    btNodeState = BTNodeStates.SUCCESS;
                    continue;
                default:
                    btNodeState = BTNodeStates.FAILURE;
                    failed = true;
                    break;
            }
        }
        return btNodeState;
    }
}

