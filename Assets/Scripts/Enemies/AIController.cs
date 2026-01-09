using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private GameObject destination;
    private NavMeshAgent agent;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private float lastAttackTime;

    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
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
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("AI attaque et inflige " + damage + " dégâts");
        destination.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

