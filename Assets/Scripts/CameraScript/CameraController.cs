using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 1.0f;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
