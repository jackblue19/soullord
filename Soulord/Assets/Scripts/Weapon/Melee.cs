using UnityEngine;

public class Melee : Weapon
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;

    public override void Attack()
    {
        if ( Time.time < nextAttackTime ) return;
        nextAttackTime = Time.time + 1f / attackRate;

        // temp circle for radar enemies -> adding raycast + l.o.s
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position , attackRange , enemyLayer);

        foreach ( Collider2D enemy in hitEnemies )
        {
            enemy.GetComponent<EnemyBase>()?.TakeDamage(damage);
        }

        // TODO -> add animation for slash
    }

    private void OnDrawGizmosSelected()
    {
        if ( attackPoint == null ) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position , attackRange);
    }
}
