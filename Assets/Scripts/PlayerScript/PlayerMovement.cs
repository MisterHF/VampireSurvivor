using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDecaySpeed = 40f;


    [Header("State")]
    private Vector2 moveInput;
    private Vector3 dashVelocity;
    private bool _dashing;

    private InputSystem_Actions action;

    //Player's layers ignore the enemy when he is sprinting and do not ignore them when he is moving.
    [Header("Layers")]
    [SerializeField] string playerLayer = "Player";
    [SerializeField] string obstacleLayer = "Enemy";


    private void Awake()
    {
        action = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        action.Player.Enable();

        action.Player.Move.performed += OnMovePerformed;
        action.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        action.Player.Move.performed -= OnMovePerformed;
        action.Player.Move.canceled -= OnMoveCanceled;

        action.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 velocity;

        if (_dashing)
        {
            dashVelocity = Vector3.MoveTowards(dashVelocity, Vector3.zero, dashDecaySpeed * Time.deltaTime);
            velocity = dashVelocity;

            if (dashVelocity.sqrMagnitude < 0.01f)
            {
                _dashing = false;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer(obstacleLayer), false);
            }
        }
        else
        {
            velocity = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        }
        transform.position += velocity * Time.deltaTime;
    }


    public void OnDash()
    {
        if (_dashing) return;

        Vector3 dir = transform.forward;

        if (moveInput.sqrMagnitude > 0.01f) dir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        dashVelocity = dir * dashSpeed;
        _dashing = true;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer(obstacleLayer), true);
    }
}
