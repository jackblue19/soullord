using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 1f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerZ>().TakeDame(damage);
            Destroy(gameObject);
        }
    }
}
