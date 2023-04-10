using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    private MovementSM _sm;
    private Animator _animator;
    private Player _player;
    private float _dashForce = 150f;
    private bool _dashAgain = false;
    private int _dashRemaining = 0;
    private bool _isDashDone = false;
    private bool _isDashComboStarted = false;
    private float _timePassed;
    private float _duration = 0.25f;
    private MakeParticlesEffect _effect;
    private FXManager _fxManager;
    private SpeechManager _speechManager;

    private const float _TIME_TO_WAIT_OFFSET = 0.05f;
    private Vector3 _RAYCAST_OFFSET = new Vector3(0f, 0.1f, 0f);

    public DashState(MovementSM machine, Animator anim, Player player, MakeParticlesEffect effect, FXManager fxManager, SpeechManager speechManager)
    {
        _sm = machine;
        _animator = anim;
        _player = player;
        _effect = effect;
        _fxManager = fxManager;
        _speechManager = speechManager;
    }

    public override void Enter()
    {
        base.Enter();
        if (_dashRemaining <= 0 || !_player.DashAgain)
            _isDashComboStarted = false;

        if(!_isDashComboStarted)
        {
            _isDashComboStarted = true;
            _dashRemaining = _player.DashCount;
            
            if(!_player.GodMode && !_player.UnlimitedStamina)
            {
                EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE,
                    new Dictionary<string, object> { { "stamina", -1 } });
                _player.CurrentStamina--;
            }
        }

        _fxManager.StopSound("dash_fx", _player.gameObject);
        _fxManager.PlaySound("dash_fx", _player.gameObject, false, 0.5f);
        
        if(Random.Range(0,20) == 0)
            _speechManager.PlaySpeech("malfunction" + Random.Range(0,6));
        else
            _speechManager.PlaySpeech("dashspeech" + Random.Range(1, 4));
        
        _animator.SetBool("IsDashing", true);
        _animator.SetTrigger("dash");
        _timePassed = 0.0f;
        _isDashDone = false;
        _sm.SkillSM.TimeToWaitAfterDash = Time.time + _duration + _TIME_TO_WAIT_OFFSET;
        _dashAgain = false;
        _effect.StartParticles();

        var currentInput = _sm.MovementInput.ToIso();

        Quaternion rotation = Quaternion.LookRotation(currentInput, Vector3.up);
        _sm.transform.rotation = rotation;
        EventManager.TriggerEvent(Events.PLAYER_MOVEMENT, new Dictionary<string, object> { { "movement", "dash" } });
    }
    float percentageCompleted;
    public override void UpdateLogic()
    {
        _timePassed += Time.deltaTime;
        percentageCompleted = _timePassed / _duration;

        RaycastHit hit;
        if(Physics.Raycast(_sm.gameObject.transform.position + _RAYCAST_OFFSET, _sm.transform.up * -1f, out hit, float.MaxValue))
        {
            if (hit.distance > 0.2f)
                _sm.gameObject.transform.position -= new Vector3(0f, hit.distance, 0f);
        }

        if (percentageCompleted >= 0.5f && _sm.IsDashing && _dashRemaining > 0 && _player.CurrentStamina > 0)
        {
            _dashAgain = true;
        }

        if (percentageCompleted >= 1.0f)
        {
            _dashRemaining--;

            if (_dashRemaining > 0)
                _player.DashAgain = true;
            else
                _player.DashAgain = false;

            _isDashDone = true;
        }

        if (_isDashDone)
        {
            if (_dashAgain)
            {
                _sm.ChangeState(_sm.DashState);
            }

            if (!_sm.IsWalking)
            {
                _sm.ChangeState(_sm.IdleState);
            }
            else
            {
                _sm.ChangeState(_sm.WalkState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.RB.AddForce(_sm.transform.forward * _dashForce, ForceMode.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
        _animator.SetTrigger("passive");
        _animator.ResetTrigger("passive");
        _animator.SetBool("IsDashing", false);
        
        _sm.RB.velocity = Vector3.zero;
    }
}
