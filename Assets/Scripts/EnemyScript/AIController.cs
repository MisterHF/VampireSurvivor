using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] Slider healthBar;

    GameObject destination;
    NavMeshAgent agent;

    private float currentHealth;
    private float lastAttackTime;

    private bool isDead;

    IObjectPool<GameObject> pool;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        OnSpawnFromPool();
    }

    void Update()
    {
        if (destination == null) return;

        float distance = Vector3.Distance(transform.position, destination.transform.position);

        if (distance > enemyData.attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(destination.transform.position);
        }
        else
        {
            agent.isStopped = true;
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50);
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < enemyData.attackCooldown)
            return;

        lastAttackTime = Time.time;
        destination.GetComponent<PlayerHealth>()?.TakeDamage(enemyData.damage);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            isDead = true;
            DropXP();
            pool.Release(gameObject);
        }
    }


    public void OnSpawnFromPool()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData missing", this);
            return;
        }

        isDead = false;
        currentHealth = enemyData.maxHealth;
        agent.speed = enemyData.moveSpeed;
        agent.angularSpeed = enemyData.angularSpeed;
        agent.acceleration = enemyData.acceleration;

        if (healthBar != null)
        {
            healthBar.maxValue = enemyData.maxHealth;
            healthBar.value = enemyData.maxHealth;
        }
    }


    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
    
    void DropXP()
    {
        if (enemyData.xpDropPrefab == null)
            return;

        GameObject drop = Instantiate(
            enemyData.xpDropPrefab,
            transform.position,
            Quaternion.identity
        );

        XPDrop xpDrop = drop.GetComponent<XPDrop>();
        if (xpDrop != null)
        {
            xpDrop.Init(enemyData.xpAmount);
        }
    }
}
