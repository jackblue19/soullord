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

    private PlayerControllerZ player;
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float stopChasingDistance = 40f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bossBulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private int bulletCount = 8;

    private Flash flash;
    private float lastShootTime;
    private EnemyPathfinding enemyPathfinding;
    private bool canSeePlayer = false;
    private KnockBack KnockBack;
    public static EnemyAIL instance;
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
        instance = this;
        flash = GetComponent<Flash>();
        KnockBack = GetComponent<KnockBack>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        player = FindAnyObjectByType<PlayerControllerZ>();
        currentHp = maxHp;
        updateHpBar();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //canSeePlayer = CanSeePlayer();
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
                if (CompareTag("SlimeBlueBoss")) // Nếu là boss
                {
                    ShootBossBulletSpread();
                }
                else // Nếu là quái thường
                {
                    ShootAtPlayer();
                }
                enemyPathfinding.MoveTo((player.transform.position - transform.position).normalized);
            }
        }
    }

    private void ShootBossBulletSpread()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject bullet = Instantiate(bossBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
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

    private void ShootAtPlayer()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            enemyBullet bulletScript = bullet.GetComponent<enemyBullet>();

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

            bulletScript.SetDamage(enterDamage);
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
        KnockBack.GetKnockBack(player.transform, 15f);
        StartCoroutine(flash.FlashRountine());
    }

    public void Die()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
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
