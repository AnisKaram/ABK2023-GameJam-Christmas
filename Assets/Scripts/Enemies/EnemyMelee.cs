public class EnemyMelee : EnemyParent
{
    private void Awake()
    {
        SetIdleDistance(idleDistance: 20f);
        SetChaseDistance(chaseDistance: 15f);
        SetAttackDistance(attackDistance: 2f);

        SetDamage(damage: 35);

        SetCoolDownTimer(cooldownTimer: 3f);

        SetMoveSpeed(moveSpeed: 4f);
    }
}