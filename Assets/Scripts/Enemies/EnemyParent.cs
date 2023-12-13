using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// TODO remove serializefield after done testing
public class EnemyParent : MonoBehaviour
{
    #region Fields
    [SerializeField] private EnemyPresenterParent _enemyPresenterParent;

    // Assign it using the spawner
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private Vector3 _spawnedPosition;
    [SerializeField] private CharacterHealth _playerHealth;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    [SerializeField] private float _distanceToIdle;
    [SerializeField] private float _distanceToChase;
    [SerializeField] private float _distanceToAttack;
    [SerializeField] private EnemyState _enemyState;

    [SerializeField] private int _damageToDeal;

    [SerializeField] private float _cooldownTimer;

    [SerializeField] private float _moveSpeed;

    [SerializeField] private bool _isCoolDownActivated;

    [SerializeField] private float _defaultHealth;
    [SerializeField] private float _health;

    private WaitForSeconds _coolDown;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _playerHealth = _playerTarget.GetComponent<CharacterHealth>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _coolDown = new WaitForSeconds(_cooldownTimer);

        _navMeshAgent.stoppingDistance = _distanceToAttack;
        _navMeshAgent.speed = _moveSpeed;
    }

    private void Update()
    {
        float distance = GetDistanceBetweenEnemyAndPlayer();

        if (distance >= _distanceToIdle)
        {
            // TODO trigger idle animation
            ChangeEnemyState(EnemyState.Idle);

            _navMeshAgent.destination = _spawnedPosition;
        }

        if (distance <= _distanceToChase)
        {
            ChangeEnemyState(EnemyState.Chase);
            
            _navMeshAgent.destination = _playerTarget.position;
        }

        if (!_isCoolDownActivated && distance <= _distanceToAttack)
        {
            ChangeEnemyState(EnemyState.Attack);

            _isCoolDownActivated = true;
            StartCoroutine(CoolDownCoroutine());

            // TODO trigger attack animation
            DealDamageToPlayer();
        }

        Debug.Log($"{_enemyState}");
    }
    #endregion

    #region Private Methods
    private float GetDistanceBetweenEnemyAndPlayer()
    {
        return Vector3.Distance(transform.position, _playerTarget.position);
    }

    private void ChangeEnemyState(EnemyState enemyState)
    {
        _enemyState = enemyState;
    }

    private void DealDamageToPlayer()
    {
        _playerHealth.TakeDamage(_damageToDeal);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, _defaultHealth);
        _enemyPresenterParent.UpdateHealthImageUI(health: _health, defaultHealth: _defaultHealth);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health < 1)
        {
            // TODO replace with something else ASAP.
            Destroy(gameObject);
            Debug.Log($"Enemy {name} is died");
        }
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return _coolDown;
        _isCoolDownActivated = false;
    }
    #endregion

    #region Protected Methods
    protected virtual void SetIdleDistance(float idleDistance)
    {
        _distanceToIdle = idleDistance;
    }

    protected virtual void SetChaseDistance(float chaseDistance)
    {
        _distanceToChase = chaseDistance;
    }

    protected virtual void SetAttackDistance(float attackDistance)
    {
        _distanceToAttack = attackDistance;
    }

    protected virtual void SetDamage(int damage)
    {
        _damageToDeal = damage;
    }

    protected virtual void SetCoolDownTimer(float cooldownTimer)
    {
        _cooldownTimer = cooldownTimer;
    }

    protected virtual void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    protected virtual void SetSpawnedPosition(Vector3 spawnedPosition)
    {
        _spawnedPosition = spawnedPosition;
    }

    protected virtual void SetEnemyHealth(int health)
    {
        _defaultHealth = health;
        _health = health;
    }
    #endregion
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}