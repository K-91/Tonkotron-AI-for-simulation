
public abstract class BTBaseNode3
{
    //The current state of the node
    protected BTNodeStates3 btNodeState;

    //Return node state
    public BTNodeStates3 BTNodeState
    {
        get { return btNodeState; }
    }

    //Evaluate the desired set of conditions 
    public abstract BTNodeStates3 Evaluate();

}

