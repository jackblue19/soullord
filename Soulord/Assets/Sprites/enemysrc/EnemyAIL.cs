using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAIL : MonoBehaviour
{
    [SerializeField] private float maxHp = 50f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private float enterDamage = 20f;
    [SerializeField] private float stayDamage = 2f;

    [SerializeField] private PlayerControllerZ player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float stopChasingDistance = 12f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;

    private float lastShootTime;
    private EnemyPathfinding enemyPathfinding;
    private bool canSeePlayer = false;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        currentHp = maxHp;
        updateHpBar();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            if (distanceToPlayer > shootingRange)
            {
                enemyPathfinding.MoveTo((player.transform.position - transform.position).normalized);
                ShootAtPlayer();
            }
            else
            {
                enemyPathfinding.MoveTo((player.transform.position - transform.position).normalized);
            }
        }
        else if (distanceToPlayer > stopChasingDistance)
        {
            enemyPathfinding.MoveTo(GetRoamingPosition());
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, obstacleLayer);
        return hit.collider == null || hit.collider.transform == player.transform;
    }

    private void ShootAtPlayer()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D is missing on EnemyBullet!");
                return;
            }

            Vector2 shootDirection = (player.transform.position - bulletSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            rb.linearVelocity = shootDirection * 10f;
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void TakeDame(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        updateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void updateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDame(enterDamage);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDame(stayDamage);
            }
        }
    }
}
