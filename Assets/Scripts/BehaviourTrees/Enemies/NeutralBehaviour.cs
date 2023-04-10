using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class NeutralBehaviour : MonoBehaviour
{
    private enum ActionState
    {
        IDLE,
        WORKING
    };

    private enum TransitionBehaviour
    {
        Default,
        Loop,
        PingPong
    }
    
    private BehaviourTree _tree;
    private NavMeshAgent _agent;
    private Animator _animator;

    private int _pingPongIndexWalk = -1;
    private bool _pingPongInverseWalk;
    
    private int _pingPongIndexWork = -1;
    private bool _pingPongInverseWork;
    
    private int _loopIndexWalk = -1;
    private int _loopIndexWork = -1;

    [SerializeField] private TransitionBehaviour _walkBehaviour;
    private readonly List<GameObject> _walkPoints = new();
    [SerializeField] private float _timeToWait = 2f;
    [Range(0, 5)] [SerializeField] private float _waitDivergence = 0.2f;
    private float _waitingTime;

    [SerializeField] private TransitionBehaviour _workBehaviour;
    private readonly List<GameObject> _workPoints = new();
    [SerializeField] private float _timeToWork = 2f;
    [Range(0, 5)] [SerializeField] private float _workDivergence = 0.5f;
    private float _workingTime;


    private ActionState _state = ActionState.IDLE;
    private Node.Status _treeStatus = Node.Status.RUNNING;
    private bool _pauseTree;
    
    private int _walkDestination;
    private int _workDestination;

    private const float STOPPING_DISTANCE = 2.0f;
    private const float WORK_STOPPING_DISTANCE = 0.5f;
    
    private int _busySlot;
    private Vector3 _busyPosition;

    [SerializeField] private float _speedMultiplier = 1.0f;
    public float SpeedMultiplier => _speedMultiplier;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void InitEnemy()
    {
        _speedMultiplier = Random.Range(0.6f, 2f);
        
        FindWalkPoints();
        FindWorkPoints();

        ResetTree();
    }

    private void FindWalkPoints()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).CompareTag("EnemyWalkPoint"))
            {
                _walkPoints.Add(transform.parent.GetChild(i).gameObject);
            }
        }
    }

    private void FindWorkPoints()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).CompareTag("EnemyWorkplace"))
            {
                _workPoints.Add(transform.parent.GetChild(i).gameObject);
            }
        }
    }
    
    private void ResetTree()
    {
        _state = ActionState.IDLE;

        _treeStatus = Node.Status.RUNNING;

        _tree = new();

        Sequence routine = new Sequence("Routine");
        
        Sequence goWalk = new Sequence("Go Walk");
        Leaf chooseWalkRoute = new Leaf("Choose Walk Route", ChooseWalkRoute);
        Leaf goToWalkLocation = new Leaf("Go To Walk Location", GoToWalkLocation);
        
        Sequence goWork = new Sequence("Go Work");
        Leaf chooseWorkRoute = new Leaf("Choose Work Route", ChooseWorkRoute);
        Leaf goToWorkLocation = new Leaf("Go To Walk Location", GoToWorkLocation);
        
        Leaf waitSomeTime = new Leaf("Wait Some Time", WaitSomeTime);
        
        goWalk.AddChild(chooseWalkRoute);
        goWalk.AddChild(goToWalkLocation);
        goWalk.AddChild(waitSomeTime);
        
        goWork.AddChild(chooseWorkRoute);
        goWork.AddChild(goToWorkLocation);
        goWork.AddChild(waitSomeTime);
        
        routine.AddChild(goWalk);
        routine.AddChild(goWork);
        
        _tree.AddChild(routine);
    }

    private Node.Status ChooseWalkRoute()
    {
        switch (_walkBehaviour)
        {
            case TransitionBehaviour.Default:
                _walkDestination = Random.Range(0, _walkPoints.Count);
                break;
            
            case TransitionBehaviour.Loop:
                _loopIndexWalk = _loopIndexWalk + 1 >= _walkPoints.Count ? 0 : _loopIndexWalk + 1;
                _walkDestination = _loopIndexWalk;
                break;
            
            case TransitionBehaviour.PingPong:
                _pingPongIndexWalk = _pingPongInverseWalk ? _pingPongIndexWalk - 1 : _pingPongIndexWalk + 1;
                if (_pingPongIndexWalk >= _walkPoints.Count)
                {
                    _pingPongIndexWalk--;
                    _pingPongInverseWalk = true;
                } else if (_pingPongIndexWalk < 0)
                {
                    _pingPongIndexWalk++;
                    _pingPongInverseWalk = false;
                }
                _walkDestination = _pingPongIndexWalk;
                break;
        }
        
        _busySlot = -1;
        
        return Node.Status.SUCCESS;
    }

    private Node.Status ChooseWorkRoute()
    {
        switch (_workBehaviour)
        {
            case TransitionBehaviour.Default:
                _workDestination = Random.Range(0, _workPoints.Count);
                break;
            
            case TransitionBehaviour.Loop:
                _loopIndexWork = _loopIndexWork + 1 >= _workPoints.Count ? 0 : _loopIndexWork + 1;
                _workDestination = _loopIndexWork;
                break;
            
            case TransitionBehaviour.PingPong:
                _pingPongIndexWork = _pingPongInverseWork ? _pingPongIndexWork - 1 : _pingPongIndexWork + 1;
                if (_pingPongIndexWork >= _workPoints.Count)
                {
                    _pingPongIndexWork--;
                    _pingPongInverseWork = true;
                } else if (_pingPongIndexWork < 0)
                {
                    _pingPongIndexWork++;
                    _pingPongInverseWork = false;
                }
                _workDestination = _pingPongIndexWork;
                break;
        }
        
        _busySlot = -1;
        
        return Node.Status.SUCCESS;
    }

    private Node.Status GoToWalkLocation()
    {
        FindFreeBusySlot(_walkPoints[_walkDestination]);
        
        if (_walkPoints.Count != 0 && _busySlot != -1)
        {
            return GoToLocation(_busyPosition);
        }
        
        return Node.Status.RUNNING;
    }

    private BusyPoint _busyPoint;
    private Node.Status GoToWorkLocation()
    {
        FindFreeBusySlot(_workPoints[_workDestination]);

        if (_workPoints.Count != 0 && _busySlot != -1)
        {
            return GoToLocation(_busyPosition, true);
        }
        
        return Node.Status.RUNNING;
    }

    private void FindFreeBusySlot(GameObject busyPoint)
    {
        if(_busySlot == -1)
        {
            _busyPoint = busyPoint.GetComponent<BusyPoint>();
            List<int> slots = _busyPoint.GetSlots;
            for (int i = 0; i < _busyPoint.MaxSlot && _busyPoint.GetSlots.Count < _busyPoint.MaxSlot; i++)
            {
                _busySlot = !slots.Contains(i) ? i : _busySlot;
            }
            if (_busySlot != -1)
            {
                _busyPoint.AddSlot(_busySlot);
            }
            else
                _waitingTime = Time.time + 4f;
            
            CalculatePosition(busyPoint);
        }
    }

    //Find and assign a position around the busy point
    private void CalculatePosition(GameObject busyPoint, float distance = 1f)
    {
        Quaternion angle = Quaternion.Euler(0,(360 / _busyPoint.MaxSlot) * _busySlot,0);
        Vector3 direction = angle * Vector3.forward;
        _busyPosition = busyPoint.transform.position + direction * distance;
    }


    private Node.Status WaitSomeTime()
    {
        if (Time.time >= _waitingTime)
        {
            _animator.SetBool("Working", false);

            if(_busyPoint != null)
                _busyPoint.RemoveSlot(_busySlot);

            if (_state == ActionState.WORKING)
            {
                _state = ActionState.IDLE;
            }

            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    private Node.Status GoToLocation(Vector3 destination, bool work = false)
    {
        destination.y = 0f;

        var stoppingDist = work ? WORK_STOPPING_DISTANCE : STOPPING_DISTANCE;
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (_state == ActionState.IDLE)
        {
            _agent.SetDestination(destination);
            _state = ActionState.WORKING;
        }
        else if (Vector3.Distance(_agent.pathEndPosition, destination) >= stoppingDist)
        {
            _state = ActionState.IDLE;
            return Node.Status.RUNNING;
        }
        else if (distanceToTarget < stoppingDist)
        {
            if(!work)
                _waitingTime = Time.time + Random.Range(_timeToWait - (_timeToWait * (_waitDivergence > 1 ? 1 : _waitDivergence)), _timeToWait + (_timeToWait * _waitDivergence));
            else
                _waitingTime = Time.time + Random.Range(_timeToWork - (_timeToWork * (_workDivergence > 1 ? 1 : _workDivergence)), _timeToWork + (_timeToWork * _workDivergence));

            _animator.SetBool("Working",work);
             
            _state = ActionState.IDLE;

            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    public void PauseTree()
    {
        _pauseTree = true;
    }

    public void UnPauseTree()
    {
        _pauseTree = false;
    }

    private void Update()
    {
        if(!_pauseTree)
        {
            if (_treeStatus == Node.Status.SUCCESS)
                ResetTree();

            _tree.Process();
        }     
    }
}
