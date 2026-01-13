using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float spawnInterval = 2f;

    [SerializeField] float forbiddenRadius = 10f; 
    [SerializeField] float spawnMaxRadius = 50f; 
    [SerializeField] float navMeshSearchRadius = 5f;

    private float timer;
    IObjectPool<GameObject>[] enemyPools;

    void Awake()
    {
        enemyPools = new IObjectPool<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            int index = i;
            enemyPools[i] = new ObjectPool<GameObject>(() => CreateEnemy(index), null, OnRelease, null, true, 10, 500);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawnEnemy();
            timer = 0f;
        }
    }

    GameObject CreateEnemy(int index)
    {
        GameObject enemy = Instantiate(enemyPrefabs[index]);
        enemy.GetComponent<AIController>()?.SetPool(enemyPools[index]);
        enemy.SetActive(false);
        return enemy;
    }

    void OnRelease(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    void TrySpawnEnemy()
    {
        const int maxAttempts = 50;

        for (int i = 0; i < maxAttempts; i++)
        {
            float radius = Random.Range(forbiddenRadius, spawnMaxRadius);
            float angle = Random.Range(0f, 360f);

            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0f, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);

            Vector3 candidate = player.position + offset;

            float sampleRadius = navMeshSearchRadius;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                float distToPlayer = Vector3.Distance(hit.position, player.position);
                if (distToPlayer < forbiddenRadius)
                {
                    continue;
                }

                GameObject enemy = enemyPools[Random.Range(0, enemyPools.Length)].Get();

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                }

                enemy.transform.position = hit.position;

                if (agent != null)
                {
                    agent.enabled = true;
                    agent.Warp(hit.position);
                    if (agent.isOnNavMesh)
                    {
                        agent.isStopped = false;
                    }
                }
                enemy.SetActive(true);

                enemy.GetComponent<AIController>()?.OnSpawnFromPool();

                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(player.position, forbiddenRadius);

        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(player.position, spawnMaxRadius);

        for (int i = 0; i < 20; i++)
        {
            float angle = i * 360f / 20;
            Vector3 p = player.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * spawnMaxRadius, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * spawnMaxRadius);
            Gizmos.DrawSphere(p, 0.2f);
        }
    }
}
