using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float distance = 5f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private float followSpeed = 10f;

    private float yaw;
    private float pitch = 20f;

    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        bool isRotating = Mouse.current.leftButton.isPressed;
        Cursor.lockState = isRotating ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isRotating;

        if (isRotating)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch - mouseDelta.y * mouseSensitivity * Time.deltaTime, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 pivot = target.position + Vector3.up * 1.5f;
        Vector3 targetPosition = pivot - rotation * Vector3.forward * distance;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.LookAt(pivot);
    }

    public Vector3 GetFlatForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        return forward.sqrMagnitude > 0.0001f ? forward.normalized : Vector3.forward;
    }

    public Vector3 GetFlatRight()
    {
        Vector3 right = transform.right;
        right.y = 0f;
        return right.sqrMagnitude > 0.0001f ? right.normalized : Vector3.right;
    }
}
