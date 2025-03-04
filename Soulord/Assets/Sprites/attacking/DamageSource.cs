using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.gameObject.GetComponent<EnemyHealthzz>() )
        {
            EnemyHealthzz enemyHealth = other.gameObject.GetComponent<EnemyHealthzz>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
