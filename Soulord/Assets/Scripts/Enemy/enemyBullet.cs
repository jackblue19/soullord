using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    private float damage;
    [SerializeField] private float lifetime = 0.5f;
    [SerializeField] private float moveSpeed = 15f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); //*Time.deltaTime
    }

    public void SetDamage(float damageAmount) 
    {
        damage = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerZ>().TakeDame(damage);
            Destroy(gameObject);
        }
    }
}
