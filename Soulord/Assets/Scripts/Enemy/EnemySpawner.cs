using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int maxEnemies = 7;
    private int currentEnemies = 0;
    private WallController wall;

    [System.Obsolete]
    private void Start()
    {
        wall = FindObjectOfType<WallController>();
        wall.LockWall(); // Khóa tường khi vào bãi quái
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for ( int i = 0; i < maxEnemies; i++ )
        {
            int randIndex = Random.Range(0 , enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randIndex] , transform.position + new Vector3(Random.Range(-3f , 3f) , Random.Range(-3f , 3f) , 0) , Quaternion.identity);
            currentEnemies++;
        }
    }

    public void OnEnemyDeath()
    {
        currentEnemies--;
        if ( currentEnemies <= 0 )
        {
            wall.UnlockWall(); // Mở tường khi tiêu diệt hết quái
        }
    }
}
