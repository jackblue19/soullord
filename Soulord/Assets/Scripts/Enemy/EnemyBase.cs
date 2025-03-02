using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 20f;
    public float moveSpeed = 2f;
    public float attackDamage = 1f;
    public float detectionRange = 5f; 
    public LayerMask playerLayer;

    protected Transform player;
    protected Rigidbody2D rb;
    protected EnemySpawner spawner; 

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spawner = FindObjectOfType<EnemySpawner>(); 
    }

    protected virtual void Update()
    {
        if ( player == null ) return;

        float distanceToPlayer = Vector2.Distance(transform.position , player.position);
        if ( distanceToPlayer <= detectionRange )
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    protected abstract void Attack();
    protected abstract void Wander();

    public void TakeDamage(float damage)
    {
        health -= damage;
        if ( health <= 0 )
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        
        LootManager.Instance.DropLoot(transform.position);

        
        if ( spawner != null )
        {
            spawner.OnEnemyDeath();
        }

        Destroy(gameObject);
    }

    protected virtual void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; 
    }
}
