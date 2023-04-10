using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node.Status;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class HostileBehaviour : MonoBehaviour
{
    protected enum ActionState
    {
        IDLE,
        WORKING
    };

    protected BehaviourTree _tree;
    private Node.Status _treeStatus = RUNNING;
    
    private ActionState _state = ActionState.IDLE;
    
    protected NavMeshAgent _agent;
    protected Animator _animator;
    
    private float _waitingTime;

    private bool _warning;
    
    private bool _hasIntimidated;
    private bool _startedIntimidating;
    private float _intimidateEnd;
    
    protected bool _inAction;
    protected float _actionEnd;
 
    private bool _isPause;

    public Transform Player { get; set; }
    protected EnemyGroup _enemyGroup;
    protected EnemySM _sm;
    protected SphereCollider _sphereCollider;
    
    private int _attackSlot = -1;
    private Vector3 _attackPosition;
    protected Player _playerScript;

    [Header("Time between possible defend")]
    [SerializeField] private float _defendCooldown = 3f;
    private float _lastDefendTime;
    [Header("1 out of x chance to defend")]
    [Range(1, 10)]
    [SerializeField] private int _defendChance = 3;

    protected Vector3 _lookDir;

    protected void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _sm = GetComponent<EnemySM>();

        _attackPosition = Player.transform.position;
        _playerScript = Player.GetComponent<Player>();
         
        ResetTree();
    }

    protected void ResetTree()
    {
        _state = ActionState.IDLE;

        _treeStatus = RUNNING;

        _tree = new();
        
        BuildBehaviour();
    }

    protected virtual void BuildBehaviour()
    {
    }
    
    protected Node.Status Intimidate()
    {
        if (_hasIntimidated)
            return FAILURE;

        _lookDir = (Player.transform.position - this.transform.position).normalized;

        if (!_startedIntimidating)
        {
            _startedIntimidating = true;
            _intimidateEnd = Mathf.Infinity;

            _animator.SetTrigger("Taunt");
        }
        else if(Time.time >= _intimidateEnd)
        {
            _startedIntimidating = false;
            _hasIntimidated = true;

            _animator.ResetTrigger("Taunt");

            if (Random.Range(0, 2) == 0)
                return SUCCESS;

            return FAILURE;
        }

        return RUNNING;
    }
    
    protected Node.Status WarnGroup()
    {
        if (_enemyGroup.GroupIsHostile && !_warning)
            return FAILURE;
        
        Vector3 groupPos = new Vector3(_enemyGroup.gameObject.transform.position.x, 0, _enemyGroup.gameObject.transform.position.z);

        if (!_inAction)
        {
            _state = ActionState.IDLE;
            _inAction = true;
            _warning = true;
            _enemyGroup.GroupIsHostile = true;
            _sphereCollider.radius += Vector3.Distance(groupPos, transform.position);
        }
        else
        {
            FindObjectOfType<FXManager>().PlaySound("enemywarn" + Random.Range(1,3), gameObject);

            if (GoToLocation(groupPos) == SUCCESS)
            {
                _enemyGroup.WarnGroup(Player);
                _inAction = false;
                _warning = false;
                
                return SUCCESS;
            }
        }

        return RUNNING;
    }
    
    //Get in melee position (axe)
    protected Node.Status GetInPosition()
    {
        if (!_inAction)
        {
            _agent.isStopped = false;
            _inAction = true;
            
            if(_attackSlot == -1)
            {
                List<int> slots = _playerScript.GetAttackerSlots;
                for (int i = 0; i < _playerScript.GetMaxAttackerSlots; i++)
                {
                    _attackSlot = !slots.Contains(i) ? i : _attackSlot;
                }
                if (_attackSlot != -1)
                {
                    _playerScript.AddAttackerSlots(_attackSlot);
                }
                else
                    _waitingTime = Time.time + 4f;
            }
        }
        else if (_attackSlot == -1)
        {
            if(!_animator.GetBool("AttackIdle"))
                _animator.SetTrigger("AttackIdle");
            
            if(WaitSomeTime() == SUCCESS)
            {
                _animator.ResetTrigger("AttackIdle");
                return SUCCESS;
            }
        }
        else
        {
            CalculatePosition();
            
            _lookDir = (Player.transform.position - transform.position).normalized;

            if (GoToLocation(_attackPosition, 0.5f) == SUCCESS)
            {
                _inAction = false;
                return SUCCESS;
            }
        }

        return RUNNING;
    }
    
    protected Node.Status DefendRetract()
    {
        _lookDir = (Player.transform.position - this.transform.position).normalized;

        if (!_animator.GetBool("AttackIdle"))
        {
            CalculatePosition(2.5f);
            
            _animator.SetTrigger("AttackIdle");
        }

        if (GoToLocation(_attackPosition, 0.5f) == SUCCESS)
            if(WaitSomeTime() == SUCCESS)
            {
                _animator.ResetTrigger("AttackIdle");

                return SUCCESS;
            }

        return RUNNING;
    }

    //Find and assign a melee attack position around the player
    protected void CalculatePosition(float distance = 1f)
    {
        Quaternion angle = Quaternion.Euler(0,(360 / _playerScript.GetAttackerSlots.Count) * _attackSlot,0);
        Vector3 direction = angle * Vector3.forward;
        _attackPosition = Player.transform.position + direction * distance;
    }

    protected Node.Status AttackPlayerMelee()
    {
        _lookDir = (Player.transform.position - this.transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        
        if (_sphereCollider.radius > _sm._chaseRadius)
            _sphereCollider.radius = _sm._chaseRadius;

        if (!_inAction && distanceToPlayer < 2f)
        {
            if(Random.Range(0, _defendChance) == 0)
            {
                if(Quaternion.Angle(Player.rotation, transform.rotation) > 160 && Time.time > _lastDefendTime)
                {
                    _lastDefendTime = Time.time + _defendCooldown;
                    _waitingTime = Time.time + 3f;

                    return FAILURE;
                }
            }
            
            _inAction = true;
            _animator.SetTrigger("Attack");
        }
        else if (Time.time >= _actionEnd)
        {
            _inAction = false;
            _animator.ResetTrigger("Attack");
            
            return SUCCESS;
        }

        return RUNNING;
    }

    protected Node.Status WaitSomeTime()
    {           
        if (Time.time >= _waitingTime)
            return SUCCESS;
        return RUNNING;
    }

    protected Node.Status GoToLocation(Vector3 destination, float withinDistance = 2f)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        
        if (_state == ActionState.IDLE)
        {
            _agent.SetDestination(destination);
            _state = ActionState.WORKING;
        }
        else if (Vector3.Distance(_agent.pathEndPosition, destination) >= withinDistance)
        {
            _state = ActionState.IDLE;
            
            return RUNNING;
        }
        else if (distanceToTarget < withinDistance)
        {
            _startedIntimidating = false;
            _state = ActionState.IDLE;
            
            return SUCCESS;
        }

        return RUNNING;
    }

    protected virtual void OnEnable()
    {
        FindObjectOfType<MusicManager>().HostileNumber++;
        
        _inAction = false;

        _attackSlot = -1;

        ResetTree();
    }

    protected virtual void OnDisable()
    {
        FindObjectOfType<MusicManager>().HostileNumber = FindObjectOfType<MusicManager>().HostileNumber < 0 ? 0 : FindObjectOfType<MusicManager>().HostileNumber - 1;
        
        if(_attackSlot != -1 && Player != null)
            Player.GetComponent<Player>().RemoveAttackSlots(_attackSlot);
        
        
        _animator.ResetTrigger("AttackIdle");
    }

    public void PauseTree()
    {
        _isPause = true;
    }

    public void UnPauseTree()
    {
        _isPause = false;
    }

    protected virtual void Update()
    {
        if (!_isPause)
        {
            if (_treeStatus == SUCCESS)
                ResetTree();

            _tree.Process();

            UpdateLookDirection();
        }
    }

    private void UpdateLookDirection()
    {
        if (_lookDir != Vector3.zero)
        {
            _lookDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(_lookDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 400f * Time.deltaTime);
        }
    }

    public EnemyGroup EnemyGroup
    { set { _enemyGroup = value; } }

    public float IntimidateEnd
    { set { _intimidateEnd = value; } }

    public float ActionEnd
    { set { _actionEnd = value; } }
}
