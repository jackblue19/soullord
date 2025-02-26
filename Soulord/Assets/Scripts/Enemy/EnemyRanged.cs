using UnityEngine;

public class EnemyRanged : EnemyBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [System.Obsolete]
    protected override void Attack()
    {
        if ( Time.time < nextAttackTime ) return;
        nextAttackTime = Time.time + attackCooldown;

        GameObject bullet = Instantiate(bulletPrefab , firePoint.position , Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = (player.position - firePoint.position).normalized * 5f;
    }

    protected override void Wander()
    {
        // Quái tầm xa cũng đứng yên khi không thấy Player
        rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if ( player != null && Vector2.Distance(transform.position , player.position) <= detectionRange )
        {
            Attack();
        }
    }
}
