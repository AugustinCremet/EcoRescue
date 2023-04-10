using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillSM : StateMachine
{
    [HideInInspector]
    public PassiveState Passive;
    [HideInInspector]
    public HeavyAttackState HeavyAttack;
    [HideInInspector]
    public LightAttackState LightAttack;
    [HideInInspector]
    public AimState Aim;
    [HideInInspector]
    public ShootState Shoot;
    [HideInInspector]
    public ChargingState Charging;
    [HideInInspector]
    public ChargeAttackState ChargeAttack;

    private PlayerInputs _playerInputs;
    private Animator _animator;

    [SerializeField] private GameObject _chargingBarObject;

    [SerializeField] private GameObject _lightEffectObject;
    [SerializeField] private GameObject _heavyEffectObject;
    [SerializeField] private GameObject _projectileEffectObject;
    [SerializeField] private GameObject _chargeEffectObject;
    [SerializeField] private GameObject _auraEffectObject;

    private FXManager _fxManager;
    private SpeechManager _speechManager;

    [SerializeField] private GameObject _lightHitBoxObject;
    [SerializeField] private GameObject _heavyHitBoxObject;

    [SerializeField] private GameObject _mousePositionObject;
    public GameObject MousePositionObject { get { return _mousePositionObject; } }

    private MovementSM _movementSM;
    public MovementSM MovementSM { get { return _movementSM; } }

    private float _timeToWaitAfterDash;
    public float TimeToWaitAfterDash { set { _timeToWaitAfterDash = value; } get { return _timeToWaitAfterDash; } }

    private bool _isLightAttack;
    public bool IsLightAttack { get { return _isLightAttack; } }

    private bool _isHeavyAttack;
    public bool IsHeavyAttack { get { return _isHeavyAttack; } }

    private bool _isAiming;
    public bool IsAiming { get { return _isAiming; } }

    private bool _isShooting;
    public bool IsShooting { get { return _isShooting; } }

    private bool _isCharging;
    public bool IsCharging { get { return _isCharging; } }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementSM = GetComponent<MovementSM>();
        Player player = GetComponent<Player>();
        MakeParticlesEffect lightEffect = _lightEffectObject.GetComponent<MakeParticlesEffect>();
        MakeParticlesEffect heavyEffect = _heavyEffectObject.GetComponent<MakeParticlesEffect>();
        MakeParticlesEffect projectileEffect = _projectileEffectObject.GetComponent<MakeParticlesEffect>();
        MakeParticlesEffect chargeEffect = _chargeEffectObject.GetComponent<MakeParticlesEffect>();
        _fxManager = FindObjectOfType<FXManager>();
        _speechManager = FindObjectOfType<SpeechManager>();

        HitBox heavyHitBox = _heavyHitBoxObject.GetComponent<HitBox>();
        HitBox lightHitBox = _lightHitBoxObject.GetComponent<HitBox>();

        Passive = new PassiveState(this, player, _animator, _fxManager, _speechManager);
        HeavyAttack = new HeavyAttackState(this, _animator, player, heavyEffect, heavyHitBox, _fxManager, _speechManager);
        LightAttack = new LightAttackState(this, _animator, player, lightEffect, lightHitBox, _fxManager, _speechManager);
        Aim = new AimState(this, _animator, player, _fxManager, _speechManager);
        Shoot = new ShootState(this, _animator, player, projectileEffect, _fxManager, _speechManager);
        Charging = new ChargingState(this, _animator, player, _chargingBarObject, _auraEffectObject, _fxManager, _speechManager);
        ChargeAttack = new ChargeAttackState(this, _animator, player, chargeEffect, _fxManager, _speechManager);

        _playerInputs = GameManager.instance.Inputs;

        _playerInputs.Skill.LightAttack.started += OnLightAttack;
        _playerInputs.Skill.LightAttack.performed += OnLightAttack;
        _playerInputs.Skill.LightAttack.canceled += OnLightAttack;

        _playerInputs.Skill.HeavyAttack.started += OnHeavyAttack;
        _playerInputs.Skill.HeavyAttack.performed += OnHeavyAttack;
        _playerInputs.Skill.HeavyAttack.canceled += OnHeavyAttack;

        _playerInputs.Skill.Special.performed += OnAim;
        _playerInputs.Skill.Special.canceled += OnAim;

        _playerInputs.Skill.Projectile.started += OnShoot;
        _playerInputs.Skill.Projectile.performed += OnShoot;
        _playerInputs.Skill.Projectile.canceled += OnShoot;

        _playerInputs.Skill.Charge.performed += OnCharge;
        _playerInputs.Skill.Charge.canceled += OnCharge;
    }

    private void OnLightAttack(InputAction.CallbackContext obj)
    {
        _isLightAttack = obj.ReadValueAsButton();
    }

    private void OnHeavyAttack(InputAction.CallbackContext obj)
    {
        _isHeavyAttack = obj.ReadValueAsButton();
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        _isAiming = obj.ReadValueAsButton();
    }

    private void OnShoot(InputAction.CallbackContext obj)
    {
        _isShooting = obj.ReadValueAsButton();
    }

    private void OnCharge(InputAction.CallbackContext obj)
    {
        _isCharging = obj.performed;
    }

    protected override BaseState GetInitialState()
    {
        return Passive;
    }

    private void OnEnable()
    {
        _playerInputs.Skill.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Skill.Disable();
    }

    public void ActivateLightHitBox()
    {
        _lightHitBoxObject.SetActive(true);
        StartCoroutine(Deactivator());
    }

    public void DeactivateLightHitBox()
    {
        _lightHitBoxObject.SetActive(false);
    }

    public void ActivateHeavyHitBox()
    {
        _heavyHitBoxObject.SetActive(true);
        StartCoroutine(Deactivator());
    }

    public void DeactivateHeavyHitBox()
    {
        _heavyHitBoxObject.SetActive(false);
    }

    public void DeactivateLightHeavyHitBoxes()
    {
        DeactivateLightHitBox();
        DeactivateHeavyHitBox();
    }

    private IEnumerator Deactivator()
    {
        yield return new WaitForSeconds(0.4f);

        DeactivateLightHeavyHitBoxes();
    }
}
