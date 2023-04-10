using UnityEngine;
using UnityEngine.AI;

public class EnemySM : StateMachine
{
    [Header("Hostility Behaviour")]
    [SerializeField] private float _hostilityRadius = 5f;
    public float _chaseRadius = 10f;

    [HideInInspector] 
    public NeutralState NeutralState;
    [HideInInspector] 
    public HostileState HostileState;
    [HideInInspector]
    public WorkState WorkState;
    [HideInInspector]
    public HitState HitState;
    [HideInInspector] 
    public DeathState DeathState;

    private NeutralBehaviour _neutralBT;
    private HostileBehaviour _hostileBT;

    private EnemyGroup _enemyGroup;
    private Enemy _enemy;

    private bool _isHostile = false;
    private bool _isStunt = false;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Rigidbody _rb;
    private SphereCollider _detectionZone;

    private void Awake()
    {
        _enemy = GetComponentInChildren<Enemy>();
        _enemyGroup = GetComponentInParent<EnemyGroup>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();

        _neutralBT = GetComponent<NeutralBehaviour>();
        _hostileBT = GetComponent<HostileBehaviour>();
        _hostileBT.enabled = false;
        
        _agent.speed = _enemy.NeutralSpeed * _neutralBT.SpeedMultiplier;

        _hostileBT.EnemyGroup = _enemyGroup;

        _detectionZone = GetComponent<SphereCollider>();

        NeutralState = new NeutralState(this);
        HostileState = new HostileState(this);
        WorkState = new WorkState(this);
        HitState = new HitState(this);
        DeathState = new DeathState(this);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("Player"))
        {
            if(!GetComponent<HostileBehaviour>().enabled)
                ActivateHostileBehaviour(other.transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DeactivateHostileBehaviour();
        }
    }

    public void ActivateHostileBehaviour(Transform player)
    {
        _isHostile = true;
        _animator.SetBool("Working", false);
        HostileBT.Player = player.transform;

        _detectionZone.radius = _chaseRadius;
        
        _agent.speed = _enemy.HostileSpeed * (_neutralBT.SpeedMultiplier < 1 ? 1 : (_neutralBT.SpeedMultiplier > 1.5f ? 1.5f : _neutralBT.SpeedMultiplier));
    }

    private void DeactivateHostileBehaviour()
    {
        _isHostile = false;
        _agent.speed = _enemy.NeutralSpeed * _neutralBT.SpeedMultiplier;
        _agent.isStopped = false;

        _detectionZone.radius = _hostilityRadius;
    }

    protected override BaseState GetInitialState()
    {
        return NeutralState;
    }

    #region PROPERTIES
    public Animator Animator => _animator;
    public NeutralBehaviour NeutralBT => _neutralBT;
    public HostileBehaviour HostileBT => _hostileBT;
    public EnemyGroup EnemyGroup => _enemyGroup;
    public Enemy Enemy => _enemy;
    public Rigidbody RB => _rb;
    public NavMeshAgent Agent => _agent;
    public bool IsHostile => _isHostile;
    public bool IsStunt
    {
        get { return _isStunt; }
        set { _isStunt = value; }
    }
    #endregion
}
