using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public float timeBetweenSpawns = 2f;
    public int maxEnemies = 7;
    private int currentEnemies = 0;
    private WallController wall;

    private void Start()
    {
        wall = FindAnyObjectByType<WallController>();

        if (wall != null)
        {
            wall.LockWall();
        }
        else
        {
            Debug.LogWarning("WallController do not exit!");
        }

        StartCoroutine( SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for ( int i = 0; i < maxEnemies; i++ )
        {
            yield return new WaitForSeconds( timeBetweenSpawns );
            int randIndex = Random.Range(0, enemyPrefabs.Length);
            int spawnIndex = Random.Range(0, spawnPoints.Length);

            Vector3 spawnPosition = spawnPoints[spawnIndex].position;
            Instantiate(enemyPrefabs[randIndex], spawnPosition, Quaternion.identity);
            currentEnemies++;
        }
    }

    public void OnEnemyDeath()
    {
        currentEnemies--;
        if ( currentEnemies <= 0 )
        {
            wall.UnlockWall(); 
        }
    }
}
