using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDecaySpeed = 40f;
    [SerializeField] float dashCooldown = 0.35f;

    [Header("Collision")]
    [SerializeField] CapsuleCollider capsule;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float skinWidth = 0.05f;
    [SerializeField] float lockedY = 0f;

    [Header("OnDashCollision")]
    [SerializeField] int enemyCollisionLayer = 7;
    [SerializeField] int playerCollisionLayer = 8;

    private Vector2 moveInput;
    private Vector3 dashVelocity;
    private bool isDashing;
    private float dashCooldownTimer;

    InputSystem_Actions action;

    void Awake()
    {
        action = new InputSystem_Actions();
    }

    void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        action.Player.Move.canceled += _ => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        action.Player.Disable();
    }

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime;
        Move();
    }
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    void Move()
    {
        Vector3 velocity;

        if (isDashing)
        {
            dashVelocity = Vector3.MoveTowards(dashVelocity, Vector3.zero, dashDecaySpeed * Time.deltaTime);

            velocity = dashVelocity;

            if (dashVelocity.sqrMagnitude < 0.01f)
            {
                isDashing = false;
                Physics.IgnoreLayerCollision(playerCollisionLayer, enemyCollisionLayer, false);
            }
        }
        else
        {
            velocity = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        }

        Vector3 displacement = velocity * Time.deltaTime;

        if (displacement.sqrMagnitude > 0f)
        {
            if (CapsuleCast(displacement.normalized, displacement.magnitude, out RaycastHit hit))
            {
                transform.position += displacement.normalized * Mathf.Max(0f, hit.distance - skinWidth);

                dashVelocity = Vector3.zero;
                if (isDashing)
                {
                    isDashing = false;
                    Physics.IgnoreLayerCollision(playerCollisionLayer, enemyCollisionLayer, false);
                }
            }
            else
            {
                transform.position += displacement;
            }
        }

        Vector3 p = transform.position;
        p.y = lockedY;
        transform.position = p;
    }


    public void OnDash()
    {
        if (isDashing || dashCooldownTimer > 0f)
            return;

        Vector3 dir = moveInput.sqrMagnitude > 0.01f ? new Vector3(moveInput.x, 0f, moveInput.y).normalized : transform.forward;

        float maxDashDistance = dashSpeed * 0.2f;

        if (CapsuleCast(dir, maxDashDistance, out RaycastHit hit)) maxDashDistance = Mathf.Max(0f, hit.distance - skinWidth);

        if (maxDashDistance <= 0f)
        {
            return;
        }

        dashVelocity = dir * (maxDashDistance / 0.2f);
        isDashing = true;
        dashCooldownTimer = dashCooldown;
        Physics.IgnoreLayerCollision(playerCollisionLayer, enemyCollisionLayer, true);
    }

    bool CapsuleCast(Vector3 dir, float dist, out RaycastHit hit)
    {
        Vector3 center = transform.position + capsule.center;
        float radius = capsule.radius;
        float height = Mathf.Max(capsule.height, radius * 2f);

        Vector3 p1 = center + Vector3.up * (height / 2f - radius);
        Vector3 p2 = center - Vector3.up * (height / 2f - radius);

        return Physics.CapsuleCast(p1, p2, radius, dir, out hit, dist, collisionMask, QueryTriggerInteraction.Ignore);
    }
}
