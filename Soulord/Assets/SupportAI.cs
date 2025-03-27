using System.Collections;
using UnityEngine;

public class SupportAI : Singleton<SupportAI>
{
    private PlayerControllerZ player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float followDistance = -3.25f;
    [SerializeField] private float followSpeed = 3.5f; // Tốc độ di chuyển theo nhân vật
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float detectRange = 10f;

    private float lastShootTime;
    private Vector3 velocity = Vector3.zero;
    private float floatHeight = 0.5f;  // Biên độ dao động
    private float floatSpeed = 2f;     // Tốc độ dao động
    private float orbitAngle = 0f;

    private void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerControllerZ>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Tạo hiệu ứng bay mượt
        Vector3 targetPosition = player.transform.position + new Vector3(followDistance, 1.5f, 0);

        targetPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;  // Hiệu ứng dao động

        // Di chuyển mượt đến vị trí mục tiêu
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Kiểm tra và bắn đạn
        GameObject enemy = FindNearestEnemy();
        if (enemy != null)
        {
            ShootAtEnemy(enemy.transform);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Slime");
        GameObject nearestEnemy = null;
        float minDistance = detectRange;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
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
}
