using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Distances")]
    [SerializeField] private float minSpawnRadius = 20f;
    [SerializeField] private float maxSpawnRadius = 40f;

    [Header("NavMesh")]
    [SerializeField] private float navMeshSearchRadius = 5f;

    private float timer;
    private IObjectPool<GameObject>[] enemyPools;


    //I need that, otherwise I would only have one type of enemy spawning after many enemy retrun in the pool
    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i] == null)
                Debug.LogError($"Enemy prefab at index {i} is null! Check your prefab assignments.");
        }

        enemyPools = new IObjectPool<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            int index = i;
            enemyPools[i] = new ObjectPool<GameObject>(() => CreateEnemy(index), OnGet, OnRelease);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawnEnemy();
            timer = 0f;
        }
    }

    private GameObject CreateEnemy(int index)
    {
        GameObject enemyGO = Instantiate(enemyPrefabs[index]);

        AIController enemyScript = enemyGO.GetComponent<AIController>();
        if (enemyScript != null)
        {
            enemyScript.SetPool(enemyPools[index]);
        }
        else
        {
            Debug.LogError($"Prefab {enemyGO.name} does not have an AIController component!");
        }

        enemyGO.SetActive(false);
        return enemyGO;
    }

    private void OnGet(GameObject enemy)
    {
        enemy.SetActive(true);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.Warp(enemy.transform.position);
    }

    private void OnRelease(GameObject enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void TrySpawnEnemy()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);

            Vector3 candidatePos = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                GameObject enemy = enemyPools[Random.Range(0, enemyPools.Length)].Get();
                enemy.transform.position = hit.position;

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.Warp(hit.position);
                }
                else
                {
                    enemy.transform.position = hit.position;
                }
                return;
            }
        }
    }
}
