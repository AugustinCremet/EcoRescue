using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Basic stats")] 
    [SerializeField] private float _speed = 7f;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private int _maxStamina = 3;
    [SerializeField] private float _staminaRefreshRate = 3.0f;
    [SerializeField] private int _dashCount = 1;

    [Header("Basic attack")] 
    [SerializeField] private int _lightAttackDamage = 30;
    [SerializeField] private int _heavyAttackDamage = 50;

    [Header("Charge attack")] 
    [SerializeField] private int _chargeAttackDamage = 100;
    [SerializeField] private float _chargeAttackCooldown = 4.0f;
    [SerializeField] private float _chargeAttackRadius = 1.0f;
    [SerializeField] private GameObject _chargeAttackObject;

    [Header("Projectile attack")] 
    [SerializeField] private int _projectileDamage = 25;
    [SerializeField] private float _projectileCooldown = 2f;
    [SerializeField] private float _projectileDistance = 10.0f;

    [Header("Power Potion Effect")]
    [SerializeField] private ParticleSystem _powerParticles;
    [SerializeField] private GameObject _secondsCanvas;
    
    [Header("Room Cleared Effect")]
    [SerializeField] private ParticleSystem _roomClearedParticles;

    private bool _hasInteract = false;

    private int _currentHealth;
    private int _currentStamina;
    private bool _dashAgain = false;

    private Animator _animator;
    private bool isDead = false;

    private float _chargeAttackCurrentCooldown;
    private float _projectileCurrentCooldown;
    private float _staminaCurrentCooldown;
    private float _dashAgainCurrentCooldown;
    private float _dashAgainTimeLimit = 1f;

    private bool _isChargeReady = true;
    private bool _isProjectileReady = true;

    private PlayerInputs _playerInputs;

    private int _nbOfCoins;
    [SerializeField] private Compass _compass;

    private List<int> _attackerSlots = new List<int>();
    private int _maxAttackerSlots = 4;

    public bool GodMode { get; private set; } = false;
    public bool UnlimitedStamina { get; private set; } = false;

    private void Awake()
    {
        float actualRadius = _chargeAttackRadius / 4f;
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;
        _animator = GetComponent<Animator>();
        _chargeAttackObject.transform.localScale = new Vector3(actualRadius, actualRadius, actualRadius);         

        _playerInputs = new PlayerInputs();
        _playerInputs.Menu.Interact.started += OnInteract;
        _playerInputs.Menu.Interact.canceled += OnInteract;

        _playerInputs.Skill.DetectEnemies.started += OnDetectEnemies;
        _playerInputs.Skill.DetectEnemies.canceled += OnDetectEnemies;

        GodMode = FindObjectOfType<UIManager>().GodMode;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        _hasInteract = obj.ReadValueAsButton();
    }

    private void OnDetectEnemies(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            _compass.UpdateCompass();
        }    
    }

    public void GainHealth(int amount)
    {
        _currentHealth += amount;
        EventManager.TriggerEvent(Events.PLAYER_HEALTH_CHANGE, new Dictionary<string, object> { { "health", amount } });
    }

    public void TakeDamage(int damage)
    {
        if (!GodMode)
        {

            _currentHealth -= damage;
            EventManager.TriggerEvent(Events.PLAYER_HEALTH_CHANGE,
                new Dictionary<string, object> { { "health", -damage } });

            if (!isDead)
                EventManager.TriggerEvent(Events.PLAYER_TAKE_DAMAGE, null);

            if (Random.Range(0, 4) == 0)
                FindObjectOfType<FXManager>().PlaySound("scream" + Random.Range(1, 3), gameObject);
            else if (Random.Range(0, 4) == 0)
                FindObjectOfType<FXManager>().PlaySound("fox_barking" + Random.Range(1, 3), gameObject);

            if (_currentHealth == 1)
                FindObjectOfType<FXManager>().PlaySound("fox_hurting1", gameObject);

            if (_currentHealth <= 0 && !isDead)
            {
                isDead = true;
                _animator.SetTrigger("death");

                GetComponent<MovementSM>().SetEnable(false);
                GetComponent<SkillSM>().SetEnable(false);

                StartCoroutine(TriggerGameOver());
            }
        }
    }

    private IEnumerator TriggerGameOver()
    {
        yield return new WaitForSeconds(3f);

        EventManager.TriggerEvent(Events.PLAYER_DIED, null);
    }

    private void Update()
    {
        if (!GodMode && !UnlimitedStamina) StaminaCD();
        else CurrentStamina = MaxStamina;
        
        ChargeAttackCD();
        ProjectileAttackCD();
        DashAgainPossibility();
    }

    private void ChargeAttackCD()
    {
        if (!_isChargeReady)
            _chargeAttackCurrentCooldown += Time.deltaTime;

        if (_chargeAttackCurrentCooldown >= _chargeAttackCooldown)
        {
            _chargeAttackCurrentCooldown = 0f;
            _isChargeReady = true;
        }
    }

    private void ProjectileAttackCD()
    {
        if (!_isProjectileReady)
            _projectileCurrentCooldown += Time.deltaTime;

        if (_projectileCurrentCooldown >= _projectileCooldown)
        {
            _projectileCurrentCooldown = 0f;
            _isProjectileReady = true;
        }
    }

    private void StaminaCD()
    {
        if (_currentStamina < _maxStamina)
        {
            _staminaCurrentCooldown += Time.deltaTime;
        }

        if (_staminaCurrentCooldown >= _staminaRefreshRate)
        {
            _staminaCurrentCooldown = 0f;
            _currentStamina++;
            EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE, new Dictionary<string, object> { { "stamina", 1 } });
        }
    }

    private void DashAgainPossibility()
    {
        if (_dashAgain)
        {
            _dashAgainCurrentCooldown += Time.deltaTime;

            if (_dashAgainCurrentCooldown > _dashAgainTimeLimit)
            {
                _dashAgain = false;
                _dashAgainCurrentCooldown = 0f;
            }
        }

        if (GodMode || UnlimitedStamina)
        {
            _dashAgain = true;
        }
    }
    
    private void PausePlayer(Dictionary<string, object> message)
    {
        var enable = !(bool)message["pause"];

        if (enable) 
            GameManager.instance.EnablePlayerInputs();
        else
            GameManager.instance.DisablePlayerInputs();

        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsDashing", false);
        _animator.SetBool("isAiming", false);
    }

    private void UseConsumable(Dictionary<string, object> message)
    {
        gameObject.GetComponent<SkillSM>().enabled = (bool)message["consumable"];
    }

    public void UsePower(bool activate)
    {
        if(activate)
        {
            _powerParticles.Play();
            _secondsCanvas.SetActive(true);
        }
        else
        {
            _powerParticles.Stop();
            _secondsCanvas.SetActive(false);
        }    

    }

    public void AddAttackerSlots(int slot)
    {
        _attackerSlots.Add(slot);
    }

    public void RemoveAttackSlots(int slot)
    {
        _attackerSlots.Remove(slot);
    }
    
    public void AddCoin(int value)
    {
        if (value != 0)
        {
            _nbOfCoins += value;

            EventManager.TriggerEvent(Events.PLAYER_COIN_CHANGE, new Dictionary<string, object> { { "coin", value } });
        }
    }

    public void InstantiateRoomClearedEffect(Dictionary<string, object> message)
    {
        _roomClearedParticles.Play();
    }

    public void TriggerGodMode(bool onButton)
    {
        if (onButton)
            GodMode = true;
        else
            GodMode = false;
    }

    public void SetUnlimitedStamina(bool value)
    {
        UnlimitedStamina = value;
        _staminaCurrentCooldown = 0f;
    }

    #region PROPERTIES
    public float Speed { get { return _speed; } set { _speed = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
    public int DashCount { get { return _dashCount; } set { _dashCount = value; } }
    public int CurrentStamina { get { return _currentStamina; } set { _currentStamina = value; } }
    public float StaminaRefreshRate { get { return _staminaRefreshRate; } set { _staminaRefreshRate = value; } }
    public int LightAttackDamage { get { return _lightAttackDamage; } set { _lightAttackDamage = value; } }
    public int HeavyAttackDamage { get { return _heavyAttackDamage; } set { _heavyAttackDamage = value; } }
    public int ChargeAttackDamage { get { return _chargeAttackDamage; } set { _chargeAttackDamage = value; } }
    public float ChargeAttackRadius { get { return _chargeAttackRadius; }
        set { float actualRadius = value / 4f;
              _chargeAttackObject.transform.localScale = new Vector3(actualRadius, actualRadius, actualRadius);} }
    public float ChargeAttackCooldown { get { return _chargeAttackCooldown; } set { _chargeAttackCooldown = value; } }
    public int ProjectileDamage { get { return _projectileDamage; } set { _projectileDamage = value; } }
    public float ProjectileCooldown { get { return _projectileCooldown; } set { _projectileCooldown = value; } }
    public float ProjectileDistance { get { return _projectileDistance; } set { _projectileDistance = value; } }
    public bool IsChargeReady { get { return _isChargeReady; } set { _isChargeReady = value; } }
    public bool IsProjectileReady { get { return _isProjectileReady; } set { _isProjectileReady = value; } }
    public bool DashAgain { get { return _dashAgain; } set { _dashAgain = value; } }
    public bool HasInteracted { get { return _hasInteract; } }
    public int NbOfCoins { get { return _nbOfCoins; } set { _nbOfCoins = value; } }
    public float PercentOfStaminaRefilled { get { return _staminaCurrentCooldown / _staminaRefreshRate; } }
    public List<int> GetAttackerSlots => _attackerSlots;
    public int GetMaxAttackerSlots => _maxAttackerSlots;
    public Compass Compass { get { return _compass; } set { } }
    public float StaminaCurrentCooldown { set { _staminaCurrentCooldown = value; } }
    #endregion

    private void OnEnable()
    {
        _playerInputs.Menu.Enable();
        _playerInputs.Skill.Enable();
        
        EventManager.StartListening(Events.PAUSE, PausePlayer);
        EventManager.StartListening(Events.PLAYER_CONSUMABLE, UseConsumable);
        EventManager.StartListening(Events.ROOM_CLEARED, InstantiateRoomClearedEffect);
    }

    private void OnDisable()
    {
        _playerInputs.Menu.Disable();
        _playerInputs.Skill.Disable();
        
        EventManager.StopListening(Events.PAUSE, PausePlayer);
        EventManager.StopListening(Events.PLAYER_CONSUMABLE, UseConsumable);
        EventManager.StopListening(Events.ROOM_CLEARED, InstantiateRoomClearedEffect);        
    }
}
