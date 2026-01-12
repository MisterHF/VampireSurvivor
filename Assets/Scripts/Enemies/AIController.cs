using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class AIController : MonoBehaviour
{
    private GameObject destination;
    private NavMeshAgent agent;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Health Settings")]
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private float maxHealth = 100f;

    private float lastAttackTime;

    private IObjectPool<GameObject> pool;


    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50);
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("AI attaque et inflige " + damage + " dégâts");
        destination.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Release();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        if (pool != null)
            pool.Release(this.gameObject);
    }
}

