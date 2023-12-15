using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyParent
{
    [SerializeField] private GameObject projectile;

    private void Awake()
    {
        SetIdleDistance(idleDistance: 30f);
        SetChaseDistance(chaseDistance: 20f);
        SetAttackDistance(attackDistance: 10f);

        SetDamage(damage: 25);

        SetCoolDownTimer(cooldownTimer: 3f);

        SetMoveSpeed(moveSpeed: 3f);

        SetSpawnedPosition(spawnedPosition: transform.position);

        SetEnemyHealth(150);
    }

    public override void DealDamageToPlayer()
    {
        Rigidbody rigidBody = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rigidBody.tag = "Projectile";

        rigidBody.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rigidBody.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
}
