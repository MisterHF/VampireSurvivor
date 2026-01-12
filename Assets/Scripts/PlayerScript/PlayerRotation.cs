using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("References & values")]
    [SerializeField] float rotationSpeed = 12f;
    [SerializeField] EnemyDetector enemyDetector;
    [SerializeField] PlayerMovement movement;

    void Update()
    {
        if (enemyDetector.CurrentTarget != null)
        {
            RotateToward(enemyDetector.CurrentTarget.position);
        }
        else
        {
            RotateTowardMovement();
        }
    }

    //Rotation via movement & without proximity to an enemy 
    void RotateTowardMovement()
    {
        Vector2 input = movement.GetMoveInput();
        if (input.sqrMagnitude < 0.01f) return;

        Vector3 dir = new Vector3(input.x, 0f, input.y);

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRot, rotationSpeed * Time.deltaTime);
    }

    //Rotation via proximity to an enemy
    void RotateToward(Vector3 worldPos)
    {
        Vector3 dir = worldPos - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
}
