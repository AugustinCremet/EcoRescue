using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISellable
{
    public string Name { get; }
    public string Description { get; }
    public int Price { get; }

}
