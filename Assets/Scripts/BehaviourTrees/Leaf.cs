using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();

    public Tick ProcessMethod;

    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }
    
    public void ResetNode()
    {
        status = new Status();
    }

    public override Status Process()
    {
        if(ProcessMethod != null)
            return ProcessMethod();
        return Status.FAILURE;
    }
}
