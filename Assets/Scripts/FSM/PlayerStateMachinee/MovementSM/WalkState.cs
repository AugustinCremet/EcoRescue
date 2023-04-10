using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    private MovementSM _sm;
    private Animator _animator;
    private Player _player;

    private float _speed = 5f;
    private const float _INITIAL_TURN_SPEED = 200f;
    private float _turnSpeed = 200f;

    private Vector3 currentInput = Vector3.zero;
    private Vector3 velo = Vector3.zero;

    private float currentDirection = 0f;
    private float currentAngle = 0f;

    public WalkState(MovementSM machine, Animator anim, Player player)
    {
        _sm = machine;
        _animator = anim;
        _player = player;
    }

    public override void Enter()
    {
        base.Enter();
        _speed = _player.Speed;
        _animator.SetBool("IsWalking", true);

        EventManager.TriggerEvent(Events.PLAYER_WALKING, new Dictionary<string, object> { { "walking", 1 } });
        EventManager.TriggerEvent(Events.PLAYER_MOVEMENT, new Dictionary<string, object> { { "movement", "walk" } });
    }

    public override void UpdateLogic()
    {
        if (!_sm.IsWalking)
        {
            _sm.ChangeState(_sm.IdleState);
        }

        base.UpdateLogic();

        if (_sm.IsDashing && _player.CurrentStamina > 0 || _sm.IsDashing && _player.DashAgain)
        {
            _sm.ChangeState(_sm.DashState);
        }
        else if(_sm.IsDashing && _player.CurrentStamina < 1)
        {
            EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE,
                    new Dictionary<string, object> { { "staminaMissing", 1 } });
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (_sm.SkillSM.GetCurrentState() is not PassiveState)
            return;

        currentInput = _sm.MovementInput.ToIso();

        currentAngle = _sm.AngleOfRotation();
        currentDirection = _sm.RotationCurve.Evaluate(currentAngle);

        _animator.SetFloat("Direction", currentDirection, 0.1f, Time.fixedDeltaTime);

        if (currentAngle > 45f || currentAngle < -45f)
        {
            _turnSpeed = 400f;
        }
        else _turnSpeed = _INITIAL_TURN_SPEED;

        if (currentInput != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(currentInput, Vector3.up);
            _sm.transform.rotation = Quaternion.RotateTowards(_sm.transform.rotation, rotation, _turnSpeed * Time.deltaTime);
        }

        // Rework Logic
        if (!(currentAngle > 90f) && !(currentAngle < -90f))
        {
            _sm.RB.MovePosition(_sm.transform.position + currentInput * _speed * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _animator.SetBool("IsWalking", false);
        EventManager.TriggerEvent(Events.PLAYER_IDLE, new Dictionary<string, object> { { "idle", 1 } });
    }
}
