using UnityEngine;
using static Node.Status;
using Vector3 = UnityEngine.Vector3;

public class HostileBehaviourElite : HostileBehaviour
{
    [Header("Object References")]
    [SerializeField] protected Transform _axeHolder;
    [SerializeField] protected Transform _gunHolder;
    [SerializeField] protected GameObject _gunAnimation;
    [SerializeField] protected GameObject _bullet;
    
    [Header("Number of bullets")]
    [SerializeField] private int _shotLeft = 6;
    
    private LayerMask _layer = 13; //Obstacle layer
    private int _layerAsLayerMask;
    private bool _couldNotSeePlayer;

    protected override void BuildBehaviour()
    {
        Selector attackRoutine = new Selector("AttackRoutine");
        Selector attackRangeChoice = new Selector("AttackRangeChoice");
        
        Leaf intimidate = new Leaf("Intimidate", Intimidate);
        Leaf warnGroup = new Leaf("Warn Group", WarnGroup);
        Leaf chooseWeapon = new Leaf("Choose a Weapon", ChooseWeapon);
        Leaf getInPosition = new Leaf("Get in Melee Position", GetInPosition);
        Leaf getInRangeAndSight = new Leaf("Get in Range and Sight of Player", GetInRangeAndSight);
        Leaf attackPlayerMelee = new Leaf("Attack Player in Melee", AttackPlayerMelee);
        Leaf attackPlayerRanged = new Leaf("Attack Player in Ranged", AttackPlayerRanged);
        Leaf defendRetract = new Leaf("Retract from melee position", DefendRetract);
        Leaf equipAxe = new Leaf("Equip Axe", EquipAxe);
        Leaf equipGun = new Leaf("Equip Gun", EquipGun);

        Selector attackPatternStart = new Selector("Attack Pattern Start");
        Sequence attackPatternMelee = new Sequence("Attack Pattern Melee");
        Sequence attackPatternRanged = new Sequence("Attack Pattern Ranged");
        Selector chooseAttackOrDefense = new Selector("Choose Attack or Defense");
        
        attackPatternStart.AddChild(warnGroup);
        attackPatternStart.AddChild(intimidate);
        
        attackPatternMelee.AddChild(equipAxe);
        attackPatternMelee.AddChild(getInPosition);
        chooseAttackOrDefense.AddChild(attackPlayerMelee);
        chooseAttackOrDefense.AddChild(defendRetract);
        attackPatternMelee.AddChild(chooseAttackOrDefense);
        
        attackPatternRanged.AddChild(chooseWeapon);
        attackPatternRanged.AddChild(getInRangeAndSight);
        attackPatternRanged.AddChild(equipGun);
        attackPatternRanged.AddChild(attackPlayerRanged);

        attackRangeChoice.AddChild(attackPatternRanged);
        attackRangeChoice.AddChild(attackPatternMelee);
        
        attackRoutine.AddChild(attackPatternStart);
        attackRoutine.AddChild(attackRangeChoice);

        _tree.AddChild(attackRoutine);
    }

    private Node.Status GetInRangeAndSight()
    {
        if (_shotLeft == 0 || _couldNotSeePlayer)
        {
            _couldNotSeePlayer = false;
            return FAILURE;
        }
        
        if (!_inAction)
        {
            _agent.isStopped = false;
            _inAction = true;
        }
        else
        {
            Vector3 dir = (transform.position - Player.position).normalized;
            float dist = Vector3.Distance(transform.position,Player.position);
            Vector3 pos = dist > 7 ? Player.position - dir * 7 :
                dist < 4 ? transform.position + dir * 4 : transform.position;
            
            _layerAsLayerMask = (1 << _layer);
            if (Physics.Linecast(pos, Player.position, _layerAsLayerMask))
            {
                _inAction = false;
                _couldNotSeePlayer = true;
                return FAILURE;
            }

            _couldNotSeePlayer = false;
            _lookDir = (Player.transform.position - transform.position).normalized;

            if (GoToLocation(pos) == SUCCESS)
            {
                _inAction = false;
                return SUCCESS;
            }
        }

        return RUNNING;
    }

    private Node.Status AttackPlayerRanged()
    {
        if (_shotLeft == 0)
            return FAILURE;
        
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        Vector3 dir = (Player.position - transform.position).normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.Rotate(new Vector3(0, 75, 0));

        if (!_inAction && distanceToPlayer < 10f)
        {
            _inAction = true;
            _animator.SetTrigger("RangedAttack");
        }
        else if (Time.time >= _actionEnd)
        {
            _inAction = false;
            _animator.ResetTrigger("RangedAttack");
            _shotLeft--;
            
            return SUCCESS;
        }

        return RUNNING;
    }
 
    private Node.Status ChooseWeapon()
    {
        if (_shotLeft > 0 && !_couldNotSeePlayer)
        {
            _axeHolder.gameObject.SetActive(false);
            _gunHolder.gameObject.SetActive(true);
        }
        else
        {
            _axeHolder.gameObject.SetActive(true);
            _gunHolder.gameObject.SetActive(false);
        }

        return SUCCESS;
    }
    
    private Node.Status EquipAxe()
    {
        _axeHolder.gameObject.SetActive(true);
        _gunHolder.gameObject.SetActive(false);

        return SUCCESS;
    }
    
    private Node.Status EquipGun()
    {
        _axeHolder.gameObject.SetActive(false);
        _gunHolder.gameObject.SetActive(true);

        return SUCCESS;
    }

    private void PlayShotgunAnimation()
    {
        _gunAnimation.GetComponent<ParticleSystem>().Play();
    }

    private void InstantiateBullet()
    {
        _gunHolder.LookAt(_playerScript.gameObject.transform);
        Instantiate(_bullet, _gunHolder.position, _gunHolder.rotation);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        
        _axeHolder.gameObject.SetActive(true);
        _gunHolder.gameObject.SetActive(false);
    }
}
