using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingState : SkillBaseState
{
    private GameObject _chargingBarObject;
    private Slider _chargingBar;
    private GameObject _auraEffectObject;
    private Color _particleColor;
    private const float _CHARGING_SPEED = 1.4f;
    private AnimationClip _chargingAnimation;

    public ChargingState(SkillSM sm, Animator animator, Player player, GameObject chargingBarObject, GameObject auraEffect, FXManager fxManager, SpeechManager speechManager)
        : base(sm, player, animator, fxManager, speechManager)
    {
        _sm = sm;
        _animator = animator;
        _chargingBarObject = chargingBarObject;
        _chargingBar = chargingBarObject.GetComponent<Slider>();
        _auraEffectObject = auraEffect;
        _particleColor = _auraEffectObject.GetComponent<ParticleSystemRenderer>().material.color;
        _fxManager = fxManager;
    }

    public override void Enter()
    {
        base.Enter();
        _chargingBarObject.SetActive(true);
        _chargingBar.value = 0f;
        _animator.SetBool("isCharging", true);
        _fxManager.PlaySound("charge_attack", _player.gameObject, false, 0.3f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(_isUsingKeyboard)
            _sm.transform.LookAt(new Vector3(_sm.MousePositionObject.transform.position.x, _sm.transform.position.y, _sm.MousePositionObject.transform.position.z));

        if (_sm.IsCharging && _sm.IsAiming)
        {
            _chargingBarObject.SetActive(true);
            _chargingBar.value += Time.deltaTime * _CHARGING_SPEED;
            _animator.SetFloat("chargingProgress", _chargingBar.value);

            _particleColor.a = _chargingBar.value;
            _auraEffectObject.SetActive(true);
        }

        if(_chargingBar.value >= 1f)
        {
            if(!_sm.IsCharging)
            {
                _auraEffectObject.SetActive(false);
                _sm.ChangeState(_sm.ChargeAttack);
            }
        }

        if(!_sm.IsAiming)
        {
            _animator.SetBool("isCharging", false);
            _auraEffectObject.SetActive(false);
            _animator.SetTrigger("passive");
            _sm.ChangeState(_sm.Passive);
        }

        if(!_sm.IsCharging && _sm.IsAiming)
        {
            _chargingBar.value -= Time.deltaTime;
            _animator.SetFloat("chargingProgress", _chargingBar.value);

            if (_chargingBar.value <= 0f)
            {
                _animator.SetBool("isCharging", false);
                _chargingBarObject.SetActive(false);
                _sm.ChangeState(_sm.Aim);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _chargingBarObject.SetActive(false);
        _auraEffectObject.SetActive(false);
        _fxManager.StopSound("charge_attack", _player.gameObject);
        if(_chargingBar.value >= 1f)
            _fxManager.PlaySound("explosion", _player.gameObject);
    }
}
