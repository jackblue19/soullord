using System.Collections;
using UnityEngine;

public class SupportAI : Singleton<SupportAI>
{
    private PlayerControllerZ player;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float followDistance = -3.25f;
    [SerializeField] private float followSpeed = 3.5f;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float detectRange = 10f;

    private float lastShootTime;
    private Vector3 velocity = Vector3.zero;
    private float floatHeight = 0.5f;
    private float floatSpeed = 2f;
    private float orbitAngle = 0f;

    private void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerControllerZ>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 targetPosition = player.transform.position + new Vector3(followDistance, 1.5f, 0);
        targetPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Tìm và tấn công kẻ địch gần nhất (cả Slime thường và Boss)
        GameObject enemy = FindNearestEnemy();
        if (enemy != null)
        {
            ShootAtEnemy(enemy.transform);
            FlipDirection(enemy.transform);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("SlimeBlueBoss");

        GameObject nearestEnemy = null;
        float minDistance = detectRange;

        // Kiểm tra Slime trước
        foreach (GameObject enemy in slimes)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // Kiểm tra Boss nếu có Boss nào gần hơn Slime
        foreach (GameObject boss in bosses)
        {
            float distance = Vector2.Distance(transform.position, boss.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = boss;
            }
        }

        return nearestEnemy;
    }

    private void ShootAtEnemy(Transform enemy)
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            Vector2 shootDirection = (enemy.position - bulletSpawnPoint.position).normalized;
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            rb.linearVelocity = shootDirection * bulletSpeed;
        }
    }

    private void FlipDirection(Transform enemy)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.flipX = enemy.position.x < transform.position.x;
    }
}
