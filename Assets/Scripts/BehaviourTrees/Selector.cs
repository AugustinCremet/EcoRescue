using static Node.Status;

public class Selector : Node
{
    public Selector(string n) => name = n;

    public override Status Process()
    {
        //Debug.Log("Node status , Selector: " + name + ", " + status);
        Status childStatus = children[currentChild].Process();
        if (childStatus == RUNNING)
            return RUNNING;
        if (childStatus == SUCCESS)
        {
            currentChild = 0;
            return SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return FAILURE;
        }

        return RUNNING;
    }
}
