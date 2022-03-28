using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector3 : BTBaseNode3
{
    /** The child nodes for this selector */
    protected List<BTBaseNode3> btNodes = new List<BTBaseNode3>();

    /** The constructor requires a lsit of child nodes to be  
     * passed in*/
    public BTSelector3(List<BTBaseNode3> btNodes)
    {
        this.btNodes = btNodes;
    }

    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override BTNodeStates3 Evaluate()
    {
        foreach (BTBaseNode3 btNode in btNodes)
        {
            switch (btNode.Evaluate())
            {
                case BTNodeStates3.FAILURE:
                    continue;
                case BTNodeStates3.SUCCESS:
                    btNodeState = BTNodeStates3.SUCCESS;
                    return btNodeState;
                default:
                    continue;
            }
        }
        btNodeState = BTNodeStates3.FAILURE;
        return btNodeState;
    }
}

