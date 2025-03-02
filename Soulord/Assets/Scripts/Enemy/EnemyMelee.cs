//using UnityEngine;

//public class EnemyMelee : EnemyBase
//{
//    public float attackRange = 1.5f;
//    private float nextAttackTime = 0f;

//    protected override void Attack()
//    {
//        if ( Time.time < nextAttackTime ) return;
//        nextAttackTime = Time.time + 1f;

//        float distance = Vector2.Distance(transform.position , player.position);
//        if ( distance <= attackRange )
//        {
//            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
//        }
//    }

//    protected override void Wander()
//    {
//        // inherit from AIL -> random walking
//        // rb.linearVelocity = Vector2.zero; // temp -> static
//    }

//    private void FixedUpdate()
//    {
//        if ( player != null && Vector2.Distance(transform.position , player.position) <= attackRange )
//        {
//            Attack();
//        }
//    }
//}
