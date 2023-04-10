using static Node.Status;

public class Sequence : Node
{
    public Sequence(string n) => name = n;

    public override Status Process()
    {
        //Debug.Log("Node status , Sequence: " + name + ", " + status);
        Status childStatus = children[currentChild].Process();
        if (childStatus == RUNNING)
            return RUNNING;
        if (childStatus == FAILURE) 
            return childStatus;

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return SUCCESS;
        }
        
        return RUNNING;
    }
}
