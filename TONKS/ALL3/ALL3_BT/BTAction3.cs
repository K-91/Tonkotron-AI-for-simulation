public class BTAction3 : BTBaseNode3
{
    //Stores the function signature for the action
    public delegate BTNodeStates3 ActionNodeFunction3();

    //Called to evaluate this node
    private ActionNodeFunction3 btAction;

    //The function is passed in and stored upon creating the action node
    public BTAction3(ActionNodeFunction3 btAction)
    {
        this.btAction = btAction;
    }

    // Evaluates the actio node
    public override BTNodeStates3 Evaluate()
    {
        switch (btAction())
        {
            case BTNodeStates3.SUCCESS:
                btNodeState = BTNodeStates3.SUCCESS;
                return btNodeState;
            case BTNodeStates3.FAILURE:
                btNodeState = BTNodeStates3.FAILURE;
                return btNodeState;
            default:
                btNodeState = BTNodeStates3.FAILURE;
                return btNodeState;
        }
    }
}

