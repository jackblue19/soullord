using UnityEngine;

public class Melee : Weapon
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;

    public override void Attack()
    {
        if ( Time.time < nextAttackTime ) return;
        nextAttackTime = Time.time + 1f / attackRate;

        // Tạo vòng tròn để kiểm tra kẻ địch trong phạm vi
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position , attackRange , enemyLayer);

        foreach ( Collider2D enemy in hitEnemies )
        {
            enemy.GetComponent<EnemyBase>()?.TakeDamage(damage);
        }

        // TODO: Thêm animation chém
    }

    private void OnDrawGizmosSelected()
    {
        if ( attackPoint == null ) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position , attackRange);
    }
}
