using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTBaseNode
{
    /** The child nodes for this selector */
    protected List<BTBaseNode> btNodes = new List<BTBaseNode>();

    /** The constructor requires a lsit of child nodes to be  
     * passed in*/
    public BTSelector(List<BTBaseNode> btNodes)
    {
        this.btNodes = btNodes;
    }

    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override BTNodeStates Evaluate()
    {
        foreach (BTBaseNode btNode in btNodes)
        {
            switch (btNode.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    continue;
                case BTNodeStates.SUCCESS:
                    btNodeState = BTNodeStates.SUCCESS;
                    return btNodeState;
                default:
                    continue;
            }
        }
        btNodeState = BTNodeStates.FAILURE;
        return btNodeState;
    }
}

