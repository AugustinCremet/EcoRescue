using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node;
using static Node.Status;
using Vector3 = UnityEngine.Vector3;

public class HostileBehaviourBoss : HostileBehaviourElite
{
    private float _throwLogElapsed;
    private Vector3 _closestLogPos;
    private GameObject _closestLogGO;
    private bool _animDone;
    [SerializeField] private Transform _logHolder;

    protected override void BuildBehaviour()
    {
        Selector attackRoutine = new Selector("AttackRoutine");
        
        Leaf intimidate = new Leaf("Intimidate", Intimidate);
        Leaf warnGroup = new Leaf("Warn Group", WarnGroup);
        Leaf getInPosition = new Leaf("Get in Melee Position", GetInPosition);
        Leaf attackPlayerMelee = new Leaf("Attack Player in Melee", AttackPlayerMelee);
        Leaf defendRetract = new Leaf("Retract from Melee Position", DefendRetract);
        Leaf findLog = new Leaf("Find a log to throw", FindLog);
        Leaf pickupLog = new Leaf("Find a log to throw", PickupLog);
        Leaf getInRangeAndSight = new Leaf("Get in range and sight of the player", GetInRangeAndSight);
        Leaf throwLog = new Leaf("Find a log to throw", ThrowLog);
        Leaf sheathAxe = new Leaf("Sheath the axe", SheathAxe);
        Leaf equipAxe = new Leaf("Equip the axe", EquipAxe);

        Selector attackPatternStart = new Selector("Attack Pattern Start");
        Sequence attackPatternMelee = new Sequence("Attack Pattern Melee");
        Selector chooseAttackOrDefense = new Selector("Choose Attack or Defense");
        Selector chooseSpecialOrNormal = new Selector("Choose Special Power or Normal Attack/Defend Pattern");
        Sequence attackPatternSpecial = new Sequence("Attack Pattern Special Power");
        
        attackPatternStart.AddChild(warnGroup);
        attackPatternStart.AddChild(intimidate);
        
        attackPatternSpecial.AddChild(findLog);
        attackPatternSpecial.AddChild(sheathAxe);
        attackPatternSpecial.AddChild(pickupLog);
        attackPatternSpecial.AddChild(getInRangeAndSight);
        attackPatternSpecial.AddChild(throwLog);
        
        attackPatternMelee.AddChild(equipAxe);
        attackPatternMelee.AddChild(getInPosition);
        chooseAttackOrDefense.AddChild(attackPlayerMelee);
        chooseAttackOrDefense.AddChild(defendRetract);
        attackPatternMelee.AddChild(chooseAttackOrDefense);
        
        chooseSpecialOrNormal.AddChild(attackPatternSpecial);
        chooseSpecialOrNormal.AddChild(attackPatternMelee);
        
        attackRoutine.AddChild(attackPatternStart);
        attackRoutine.AddChild(chooseSpecialOrNormal);

        _tree.AddChild(attackRoutine);
    }
    
    private Status FindLog()
    {
        if (!_inAction)
        {
            _inAction = true;

            _closestLogPos = new Vector3(50, 50, 50);

            foreach (Transform t in transform.parent)
            {
                if (t.CompareTag("BossLog"))
                {
                    if (Vector3.Distance(transform.position, t.transform.position) <
                        Vector3.Distance(transform.position, _closestLogPos))
                    {
                        _closestLogPos = t.transform.position;
                        _closestLogGO = t.gameObject;
                    }
                }
            }
        }
        else if (Vector3.Distance(transform.position, _closestLogPos) < 10)
        {
            if (GoToLocation(_closestLogPos) == SUCCESS)
            {
                transform.LookAt(_closestLogPos);        
                _inAction = false;
                return SUCCESS;
            }
        }
        else
        {
            _inAction = false;
            return FAILURE;
        }
        
        return RUNNING;
    }
    
    //event in Boss/Lifting animation
    private void LogPicked()
    {
        Rigidbody _rb = _closestLogGO.GetComponent<Rigidbody>();
        
        _logHolder.gameObject.SetActive(true);
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _closestLogGO.GetComponentInChildren<NavMeshObstacle>().enabled = false;
        _closestLogGO.transform.SetParent(_logHolder);
        _closestLogGO.transform.localPosition = Vector3.zero;
        _closestLogGO.transform.localRotation = new Quaternion(0, 0, 0, 1);
    }
    
    private Status PickupLog()
    {
        if (!_inAction)
        {
            _inAction = true;
            _lookDir = _closestLogPos;
            _animator.SetTrigger("LiftLog");
        }
        else if(_animDone)
        {
            _animator.ResetTrigger("LiftLog");
            _inAction = false;
            _animDone = false;
            return SUCCESS;
        }
        
        return RUNNING;
    }

    private Status GetInRangeAndSight()
    {
        if (!_inAction)
        {
            _agent.isStopped = false;
            _inAction = true;
            _lookDir = _playerScript.transform.position;
        }
        else
        {
            List<int> slots = _playerScript.GetAttackerSlots;
            int maxSlots = _playerScript.GetMaxAttackerSlots;
            
            if(slots.Count != 0)
            {
                Vector3 attackPosition = new Vector3(1000, 1000, 1000);
                for(int i = 1; i <= maxSlots; i++)
                {
                    Quaternion angle = Quaternion.Euler(0, (360 / slots.Count) * (i - 0.5f), 0);
                    Vector3 direction = angle * Vector3.forward;
                    Vector3 newPosition = Player.transform.position + direction * 5f;
                    if (Vector3.Distance(newPosition, transform.position) <
                        Vector3.Distance(attackPosition, transform.position))
                        attackPosition = newPosition;
                }

                if (GoToLocation(attackPosition, 2.5f) == SUCCESS)
                {
                    _inAction = false;
                    return SUCCESS;
                }
                    
                return RUNNING;
            }

            _inAction = false;
            return SUCCESS;
        }

        return RUNNING;
    }

    //event call by boss animations
    private void AnimDone()
    {
        _animDone = true;
    }

    //event in Boss/Sheath animation
    private void HideAxe()
    {
        _axeHolder.gameObject.SetActive(false);
    }
    
    private Status SheathAxe()
    {
        if (!_inAction)
        {
            if (!_axeHolder.gameObject.activeSelf)
                return SUCCESS;

            _inAction = true;
            _animator.SetTrigger("SheathAxe");
        }
        else if(_animDone)
        {
            _animator.ResetTrigger("SheathAxe");
            _inAction = false;
            _animDone = false;
            return SUCCESS;
        }
        
        return RUNNING;
    }

    private Status EquipAxe()
    {
        if (!_inAction)
        {
            if (_axeHolder.gameObject.activeSelf)
                return SUCCESS;

            _inAction = true;
            _animator.SetTrigger("SheathAxe");
        }
        else if(_animDone)
        {
            _animator.ResetTrigger("SheathAxe");
            _inAction = false;
            _animDone = false;
            _axeHolder.gameObject.SetActive(true);
            return SUCCESS;
        }
        
        return RUNNING;
    }

    
    private Status ThrowLog()
    {
        if (!_inAction)
        {
            _inAction = true;
            _animator.SetTrigger("ThrowLog");
        }
        else if(_animDone)
        {
            _animator.ResetTrigger("ThrowLog");
            _inAction = false;
            _animDone = false;
            return SUCCESS;
        }
        
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.LookAt(Player.transform);
        return RUNNING;
    }
    
    //event in Boss/Throwing animation
    private void DropLog()
    {
        Rigidbody _rb = _closestLogGO.GetComponent<Rigidbody>();
        
        _closestLogGO.transform.SetParent(transform.parent);
        Vector3 dir = (Player.transform.position - _closestLogGO.transform.position).normalized;
        _rb.useGravity = true;
        _rb.AddForce(dir * 1000, ForceMode.Force);
        _rb.constraints = RigidbodyConstraints.None;
        _closestLogGO.GetComponent<BoxCollider>().enabled = true;
    }
}
