using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] normalEnemiesSample;
    [SerializeField] private GameObject[] bossEnemiesSample;
    [SerializeField] private GameObject[] normalEnemiesSeaMap;
    [SerializeField] private GameObject[] bossEnemiesSeaMap;

    [SerializeField] private Transform[] spawnPoints; // Điểm spawn quái thường
    [SerializeField] private Transform[] bossSpawnPoints; // Điểm spawn boss

    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private float timeBetweenWaves = 2f;

    private GameObject[] normalEnemies;
    private GameObject[] bossEnemies;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private GameTimer gameTimer;
    public AreaExit exit;

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        LoadEnemyLists();
    }

    private void LoadEnemyLists()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "SampleScence")
        {
            normalEnemies = normalEnemiesSample;
            bossEnemies = bossEnemiesSample;
        }
        else if (sceneName == "seaMap")
        {
            normalEnemies = normalEnemiesSeaMap;
            bossEnemies = bossEnemiesSeaMap;
        }
    }

    public void PlayerMoved()
    {
        gameTimer?.StartTimer();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (currentWave = 0; currentWave < 3; currentWave++)
        {
            Debug.Log($"Bắt đầu round {currentWave + 1}");
            yield return StartCoroutine(SpawnRound(currentWave));
            yield return new WaitForSeconds(timeBetweenWaves); // Chờ 2 giây giữa các round
        }
    }

    private IEnumerator SpawnRound(int waveIndex)
    {
        enemiesRemaining = 8 + 1; // 8 quái normal + 1 boss

        // Random 2 quái trong danh sách normal
        if (normalEnemies.Length >= 2)
        {
            int firstEnemyIndex = Random.Range(0, normalEnemies.Length);
            int secondEnemyIndex;
            do
            {
                secondEnemyIndex = Random.Range(0, normalEnemies.Length);
            } while (secondEnemyIndex == firstEnemyIndex); // Đảm bảo không trùng

            GameObject firstEnemy = normalEnemies[firstEnemyIndex];
            GameObject secondEnemy = normalEnemies[secondEnemyIndex];

            // Spawn mỗi con 4 lần => Tổng 8 lần
            for (int i = 0; i < 4; i++)
            {
                SpawnEnemy(firstEnemy, false);
                yield return new WaitForSeconds(timeBetweenSpawns);

                SpawnEnemy(secondEnemy, false);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
        else
        {
            Debug.LogError("Không đủ quái normal trong danh sách!");
        }

        // Spawn 1 boss
        if (waveIndex < bossEnemies.Length)
        {
            SpawnEnemy(bossEnemies[waveIndex], true);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }


    private void SpawnEnemy(GameObject enemyPrefab, bool isBoss)
    {
        Transform[] spawnArray = isBoss ? bossSpawnPoints : spawnPoints;
        if (spawnArray.Length == 0) return; // Tránh lỗi nếu chưa thiết lập điểm spawn

        int spawnIndex = Random.Range(0, spawnArray.Length);
        Vector3 spawnPosition = spawnArray[spawnIndex].position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemyAIL>().SetSpawner(this);
    }

    public void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (currentWave >= 2) // Nếu đã qua 3 round
            {
                if (SceneManager.GetActiveScene().name == "seaMap")
                {
                    gameTimer.StopTimer();
                    gameTimer.WinningBonus();
                    gameTimer.SaveResult(true);
                    gameTimer.ShowSuccessScreen(true);
                }
                exit.UnlockExit();
            }
        }
    }

    public void GameOver()
    {
        gameTimer.StopTimer();
        gameTimer.SaveResult(false);
        gameTimer.ShowSuccessScreen(false);
    }
}
