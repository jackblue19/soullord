using UnityEngine;

public class SupportBullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f;
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
        if (collision.CompareTag("Slime") || collision.gameObject.CompareTag("SlimeBlueBoss"))
        {
            collision.GetComponent<EnemyAIL>().TakeDame(damage);
            Destroy(gameObject);
        }
    }
}
