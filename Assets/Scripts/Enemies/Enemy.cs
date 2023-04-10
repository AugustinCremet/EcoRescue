using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _enemyValue = 3;
    [SerializeField] private int stuntThreshold = 40;
    [SerializeField] private GameObject _coinsPrefab;
    [SerializeField] private EnemyUI _damageUI;
    [SerializeField] private GameObject _hitbox;

    private float _neutralSpeed = 2f;
    private float _hostileSpeed = 3.5f;

    private int _currentHealth;
    private int damageCounter = 0;
    private bool _isDead = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;
        damageCounter += damage;

        if (damageCounter >= stuntThreshold)
        {
            _animator.SetTrigger("Hit");
            damageCounter = 0;
        }

        _damageUI.SpawnDamageUI(damage);

        if (_currentHealth <= 0)
        {
            _hitbox.SetActive(false);

            SpawnCollectables();

            GameManager.instance.ActiveDungeonManager.RemoveActiveEnemy(this.gameObject);
            _isDead = true;

            EventManager.TriggerEvent(Events.ENEMY_KILLED, new Dictionary<string, object> { { "coin", _enemyValue } });
        }
        else
        {
            FindObjectOfType<FXManager>().PlaySound("enemyhurt" + Random.Range(1, 11), transform.parent.gameObject);
        }
    }

    private void SpawnCollectables()
    {
        for (int i = 0; i < _enemyValue; i++)
        {
            var coin = Instantiate(_coinsPrefab);
            coin.transform.position = this.transform.position;

            Helpers.MoveToScene(coin, GameManager.instance.ActiveDungeonManager.ActiveRoom.name);

            var direction = new Vector3(Random.Range(-1, 1), 1f, Random.Range(-1, 1));
            coin.GetComponent<Rigidbody>().AddForce(direction * 3f, ForceMode.Impulse);
        }
    }

    #region PROPRETIES
    public bool Dead => _isDead;
    public int Health => _currentHealth;
    public float NeutralSpeed => _neutralSpeed;
    public float HostileSpeed => _hostileSpeed;
    #endregion  
}
