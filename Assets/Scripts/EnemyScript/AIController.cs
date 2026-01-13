using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    GameObject destination;
    NavMeshAgent agent;

    [Header("Attack Settings")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] int damage = 10;
    [SerializeField] float attackCooldown = 1f;

    [Header("Health Settings")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] Slider healthBar;

    float currentHealth;
    float lastAttackTime;

    IObjectPool<GameObject> pool;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (destination == null) return;

        float distance = Vector3.Distance(transform.position, destination.transform.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(destination.transform.position);
        }
        else
        {
            agent.isStopped = true;
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.H)) { TakeDamage(50); }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        destination.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
            pool.Release(gameObject);
    }
    public void OnSpawnFromPool()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }


    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }
}
