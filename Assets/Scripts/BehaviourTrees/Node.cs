using System.Collections.Generic;

public class Node
{
    public enum Status
    {
        SUCCESS,
        RUNNING,
        FAILURE
    };
    public Status status;
    public List<Node> children = new();
    public int currentChild = 0;
    public string name;

    public Node() { }
    
    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}
