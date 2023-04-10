using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseState 
{
    protected bool _isUsingKeyboard;
    public BaseState() {}

    public virtual void Enter() 
    {
        _isUsingKeyboard = ControlChangeInGame.Instance.IsUsingKeyboard;
    }
    public virtual void UpdateLogic() 
    {
        _isUsingKeyboard = ControlChangeInGame.Instance.IsUsingKeyboard;
    }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
