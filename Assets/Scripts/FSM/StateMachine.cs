using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;
    private GameObject _mousPositionObject;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null)
            _currentState.Enter();
    }

    private void Update()
    {
        if (_currentState != null)
            _currentState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        if (_currentState != null)
            _currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();

        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    public BaseState GetCurrentState()
    {
        return _currentState;
    }

    public void ResetState()
    {
        if (GetInitialState() != null)
            ChangeState(GetInitialState());
    }
}
