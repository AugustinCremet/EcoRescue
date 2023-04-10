using MalbersAnimations.Conditions;
using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementSM : StateMachine
{
    [HideInInspector]
    public IdleState IdleState;
    [HideInInspector]
    public WalkState WalkState;
    [HideInInspector]
    public DashState DashState;

    [SerializeField] private GameObject _dashEffectObject;
    [SerializeField] private AnimationCurve _rotationCurve;
    public AnimationCurve RotationCurve => _rotationCurve;

    private SkillSM _skillSM;
    public SkillSM SkillSM
        { get { return _skillSM; } }

    private PlayerInputs _playerInputs;

    private Player _player;

    private Animator _animator;

    private Rigidbody _rb;
    public Rigidbody RB
        { get { return _rb; } }

    private Vector3 _movementInput;
    public Vector3 MovementInput => _movementInput;

    public bool IsWalking => _movementInput != Vector3.zero;

    private bool isDashing;
    //public bool IsDashing => isDashing;
    public bool IsDashing { set{ isDashing = value;} get { return isDashing; } }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _skillSM = GetComponent<SkillSM>();
        _player = GetComponent<Player>();
        MakeParticlesEffect dashEffect = _dashEffectObject.GetComponent<MakeParticlesEffect>();

        IdleState = new IdleState(this, _animator);
        WalkState = new WalkState(this, _animator, _player);
        DashState = new DashState(this, _animator, _player, dashEffect, FindObjectOfType<FXManager>(), FindObjectOfType<SpeechManager>());

        _playerInputs = GameManager.instance.Inputs;

        _playerInputs.Movement.Walk.performed += OnMovement;
        _playerInputs.Movement.Walk.canceled += OnMovement;
        _playerInputs.Movement.Walk.started += OnMovement;

        _playerInputs.Movement.Dash.performed += OnDash;
        _playerInputs.Movement.Dash.canceled += OnDash;
        _playerInputs.Movement.Dash.started += OnDash;
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        var input = obj.ReadValue<Vector2>();
        _movementInput = new Vector3(input.x, 0, input.y);
    }

    private void OnDash(InputAction.CallbackContext obj)
    {    
        isDashing = obj.ReadValueAsButton();
    }

    public float AngleOfRotation()
    {
        var targetposition = transform.position + MovementInput.ToIso();
        var targetDir = targetposition - transform.position;
        var forward = transform.forward;

        float desiredAngle = Vector3.SignedAngle(forward, targetDir, Vector3.up);

        return desiredAngle;
    }

    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    private void OnEnable()
    {
        _playerInputs.Movement.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Movement.Disable();
    }
}
