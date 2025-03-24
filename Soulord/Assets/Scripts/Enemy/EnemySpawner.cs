using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] wave1Enemies; // Đợt 1
    [SerializeField] private GameObject[] wave2Enemies; // Đợt 2
    [SerializeField] private GameObject[] boss;           // Đợt 3 (Boss)
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private float timeBetweenWaves = 2f; // Thời gian nghỉ giữa các đợt
    [SerializeField] private int enemiesRemaining = 0; // Số quái còn lại

    [SerializeField] private int currentWave = 1;
    public AreaExit exit;
    private GameTimer gameTimer;
    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
    }

    public void PlayerMoved()
    {
        gameTimer?.StartTimer();
        StartCoroutine(SpawnWave1());
    }

    private IEnumerator SpawnWave1()
    {
        yield return StartCoroutine(SpawnEnemies(wave1Enemies, 5)); // Đợt 1: 5 quái
        currentWave = 1;
    }

    private IEnumerator SpawnWave2()
    {
        yield return StartCoroutine(SpawnEnemies(wave2Enemies, 4)); // Đợt 2: 4 quái mạnh hơn
        currentWave = 2;
    }

    private IEnumerator SpawnBoss()
    {
        yield return StartCoroutine(SpawnEnemies(boss, 1));
        currentWave = 3;
    }

    private IEnumerator SpawnEnemies(GameObject[] enemyPrefabs, int count)
    {
        enemiesRemaining = count;
        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, enemyPrefabs.Length);
            SpawnEnemy(enemyPrefabs[randIndex]);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnIndex].position;
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyAIL>().SetSpawner(this);
    }

    public void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (currentWave == 1)
            {
                StartCoroutine(StartNextWave(SpawnWave2()));
            }
            else if (currentWave == 2)
            {
                StartCoroutine(StartNextWave(SpawnBoss()));
            }
            else if (currentWave == 3)
            {
                if (SceneManager.GetActiveScene().name == "seaMap")
                {
                    gameTimer.StopTimer();
                    gameTimer.SaveResult(true);
                }
                exit.UnlockExit();
            }
        }
    }

    public void GameOver()
    {
        gameTimer.StopTimer();
        gameTimer.SaveResult(false); // Lưu kết quả thua
    }

    private IEnumerator StartNextWave(IEnumerator nextWave)
    {
        Debug.Log("Tất cả quái đã chết, chờ " + timeBetweenWaves + " giây...");
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(nextWave);
    }
}
