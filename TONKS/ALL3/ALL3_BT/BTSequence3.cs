using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence3 : BTBaseNode3
{
    //The children of the sequencer
    protected List<BTBaseNode3> btNodes = new List<BTBaseNode3>();

    //Children set through constructor
    public BTSequence3(List<BTBaseNode3> btNodes)
    {
        this.btNodes = btNodes;
    }

    //If any child node returns a failure, the entire node fails. 
    public override BTNodeStates3 Evaluate()
    {
        bool failed = false;
        foreach (BTBaseNode3 btNode in btNodes)
        {
            if (failed == true)
            {
                break;
            }

            switch (btNode.Evaluate())
            {
                case BTNodeStates3.FAILURE:
                    btNodeState = BTNodeStates3.FAILURE;
                    failed = true;
                    break;
                case BTNodeStates3.SUCCESS:
                    btNodeState = BTNodeStates3.SUCCESS;
                    continue;
                default:
                    btNodeState = BTNodeStates3.FAILURE;
                    failed = true;
                    break;
            }
        }
        return btNodeState;
    }
}

