using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyParent : MonoBehaviour
{
    #region Fields
    [Header("Scripts")]
    [SerializeField] private EnemyPresenterParent _enemyPresenterParent;

    [Header("Animators")]
    [SerializeField] private Animator _animator;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem _dieEffect;

    [Header("Transforms")]
    [SerializeField] private Transform _playerTarget; // TODO Assign it using the spawner

    private CharacterHealth _playerHealth;

    private Vector3 _spawnedPosition;

    private NavMeshAgent _navMeshAgent;
    
    private float _distanceToIdle;
    private float _distanceToChase;
    private float _distanceToAttack;

    private int _damageToDeal;

    private float _cooldownTimer;

    private float _moveSpeed;

    private bool _isCoolDownActivated;

    private float _defaultHealth;
    private float _health;

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

        // Idle mode.
        if (distance >= _distanceToIdle)
        {
            MoveAgentTowardsTarget(targetPosition: _spawnedPosition);
        }

        // Chase mode.
        if (distance <= _distanceToChase)
        {
            MoveAgentTowardsTarget(targetPosition: _playerTarget.position);
        }

        // Attack mode.
        if (!_isCoolDownActivated && distance <= _distanceToAttack)
        {
            _isCoolDownActivated = true;
            StartCoroutine(CoolDownCoroutine());
            TriggerAttackAnimation();
            DealDamageToPlayer();
        }
    }
    #endregion

    #region Public Methods
    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0, _defaultHealth);

        _enemyPresenterParent.UpdateHealthImageUI(health: _health, defaultHealth: _defaultHealth);

        CheckHealth();
    }
    #endregion

    #region Private Methods
    private float GetDistanceBetweenEnemyAndPlayer()
    {
        return Vector3.Distance(transform.position, _playerTarget.position);
    }

    private void MoveAgentTowardsTarget(Vector3 targetPosition)
    {
        _navMeshAgent.destination = targetPosition;
    }

    private void DealDamageToPlayer()
    {
        _playerHealth.TakeDamage(_damageToDeal);
    }

    private void CheckHealth()
    {
        if (_health < 1)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        ParticleSystem particleSystem = Instantiate(_dieEffect);
        particleSystem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        particleSystem.Play();

        Destroy(gameObject);
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return _coolDown;
        _isCoolDownActivated = false;
    }

    private void TriggerAttackAnimation()
    {
        _animator.SetTrigger("Attack");
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