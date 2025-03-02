using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject , lifeTime); // bullet lifespan
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Enemy") )
        {
            collision.GetComponent<EnemyBase>()?.TakeDamage(damage);
            Destroy(gameObject); 
        }
        else if ( collision.CompareTag("Wall") )
        {
            Destroy(gameObject); // if special buff => remove destroy -> bounce bullet
        }
    }
}
