using System.Collections;
using UnityEngine;

// TODO remove serializefield after done testing
public class EnemyParent : MonoBehaviour
{
    // Assign it using the spawner
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private CharacterHealth _playerHealth;
    
    [SerializeField] private float _distanceToIdle;
    [SerializeField] private float _distanceToChase;
    [SerializeField] private float _distanceToAttack;
    [SerializeField] private EnemyState _enemyState;

    [SerializeField] private int _damageToDeal;

    [SerializeField] private float _cooldownTimer;

    [SerializeField] private float _moveSpeed;

    [SerializeField] private bool _isCoolDownActivated;

    private WaitForSeconds _coolDown;

    private void Start()
    {
        _playerHealth = _playerTarget.GetComponent<CharacterHealth>();
        _coolDown = new WaitForSeconds(_cooldownTimer);
    }

    private void Update()
    {
        float distance = GetDistanceBetweenEnemyAndPlayer();

        if (distance >= _distanceToIdle)
        {
            ChangeEnemyState(EnemyState.Idle);
        }

        if (distance <= _distanceToChase)
        {
            ChangeEnemyState(EnemyState.Chase);
            // The move towards test passed.
            // TODO we have to use the AI agent to move the enemy with same logic.
            //MoveTowardsSpecificTarget(target: _playerTarget);
        }

        if (!_isCoolDownActivated && distance <= _distanceToAttack)
        {
            ChangeEnemyState(EnemyState.Attack);

            _isCoolDownActivated = true;
            StartCoroutine(CoolDownCoroutine());

            DealDamageToPlayer();
        }

        Debug.Log($"{_enemyState}");
    }

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

    private IEnumerator CoolDownCoroutine()
    {
        yield return _coolDown;
        _isCoolDownActivated = false;
    }

    private void MoveTowardsSpecificTarget(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > _distanceToAttack) // To prevent overshooting.
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;

            float rotateSpeed = 15f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            //SetWalkingAnimation(isWalking: true); TODO add later
        }
    }

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
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}