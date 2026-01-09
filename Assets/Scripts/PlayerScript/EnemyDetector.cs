using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("Sphere Detection")]
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask enemyLayer;

    public Transform CurrentTarget { get; private set; }

    void Update()
    {
        DetectClosestEnemy();
    }

    void DetectClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider enemy in enemies)
        {
            float sqrDist = (enemy.transform.position - transform.position).sqrMagnitude;
            if (sqrDist < closestDist)
            {
                closestDist = sqrDist;
                closest = enemy.transform;
            }
        }

        CurrentTarget = closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
