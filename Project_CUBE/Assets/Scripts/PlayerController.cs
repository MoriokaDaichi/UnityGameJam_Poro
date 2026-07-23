using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 360.0f; // 度/秒
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;

        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1.0f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1.0f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1.0f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1.0f;
        }

        Vector3 camForward = thirdPersonCamera != null ? thirdPersonCamera.GetFlatForward() : Vector3.forward;
        Vector3 camRight = thirdPersonCamera != null ? thirdPersonCamera.GetFlatRight() : Vector3.right;

        Vector3 moveDirection = camForward * vertical + camRight * horizontal;
        if (moveDirection.sqrMagnitude > 1.0f)
        {
            moveDirection.Normalize();
        }
        bool isMoving = moveDirection.sqrMagnitude > 0.0f;
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetBool("IsGrounded", controller.isGrounded);
        }

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2.0f;
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
            }
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 motion = moveDirection * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(motion * Time.deltaTime);
    }
}
