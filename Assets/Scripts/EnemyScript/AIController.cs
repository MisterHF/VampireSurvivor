using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;
using System.Collections;

public class AIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] Slider healthBar;

    GameObject destination;
    NavMeshAgent agent;

    private float currentHealth;
    private float lastAttackTime;
    private Coroutine attackCoroutine;
    private bool isAttacking;
    private Transform lockedTarget;

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
        if (destination == null || isDead || isAttacking) { return;}

        float distance = Vector3.Distance(transform.position, destination.transform.position);

        if (distance > enemyData.attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(destination.transform.position);
        }
        else
        {
            Attack();
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50);
        }
    }

    void Attack()
    {
        if (isAttacking || Time.time - lastAttackTime < enemyData.attackCooldown) { return;}

        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public void TakeDamage(float amount)
    {
        if (isDead) { return;}

        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

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
        isAttacking = false;
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
        if (XpSpawner.Instance == null)
            return;

        XpSpawner.Instance.SpawnXp(transform.position);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        lockedTarget = destination.transform;

        float startTime = Time.time;

        while (Time.time - startTime < enemyData.attackWindUp)
        {
            yield return null;
        }
        
        if (!isDead && lockedTarget != null)
        {
            float distance = Vector3.Distance(transform.position, lockedTarget.position);
            if (distance <= enemyData.attackRange)
            {
                lockedTarget.GetComponent<PlayerHealth>()?.TakeDamage(enemyData.damage);
                lastAttackTime = Time.time;
            }
        }
        isAttacking = false;
    }
}
