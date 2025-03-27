using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private float maxHp = 200f; // Máu trâu hơn
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private float enterDamage = 30f;
    [SerializeField] private float stayDamage = 5f;

    private PlayerControllerZ player;
    [SerializeField] private float shootingRange = 12f;
    [SerializeField] private float stopChasingDistance = 50f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 3f;
    [SerializeField] private int bulletCount = 8; // Số đạn tỏa ra

    private Flash flash;
    private float lastShootTime;
    private EnemyPathfinding enemyPathfinding;
    private KnockBack knockBack;
    private EnemySpawner spawner;

    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }
    }

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        player = FindAnyObjectByType<PlayerControllerZ>();
        currentHp = maxHp;
        UpdateHpBar();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < shootingRange)
        {
            enemyPathfinding.MoveTo((player.transform.position - transform.position).normalized);
        }
        else
        {
            if (distanceToPlayer > stopChasingDistance)
            {
                enemyPathfinding.MoveTo(GetRoamingPosition());
            }
            else
            {
                ShootBulletSpread(); // Bắn đạn tỏa
                enemyPathfinding.MoveTo((player.transform.position - transform.position).normalized);
            }
        }
    }

    private void ShootBulletSpread()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                EnemyBossBullet bulletScript = bullet.GetComponent<EnemyBossBullet>();

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = shootDirection * 8f;
                }

                bulletScript.SetDamage(enterDamage);
            }
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        knockBack.GetKnockBack(player.transform, 2f); // Giật lùi nhẹ hơn
        StartCoroutine(flash.FlashRountine());
    }

    public void Die()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHpBar()
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
            player?.TakeDame(enterDamage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player?.TakeDame(stayDamage);
        }
    }
}
