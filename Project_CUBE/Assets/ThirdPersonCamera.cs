using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPosition =
            target.position + target.TransformDirection(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 lookPosition = target.position + Vector3.up * 1.5f;
        transform.LookAt(lookPosition);
    }
}
