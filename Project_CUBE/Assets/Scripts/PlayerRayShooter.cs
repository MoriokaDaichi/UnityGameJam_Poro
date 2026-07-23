using UnityEngine;
using UnityEngine.InputSystem;

// エイム中(右クリック)に左クリックでカメラ正面へ光線を発射し、Receiverに命中させる
public class PlayerRayShooter : MonoBehaviour
{
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform firePoint; // 未設定ならプレイヤー自身の少し上から発射
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float beamDisplayDuration = 0.15f;

    private float beamTimer = 0f;

    void Awake()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (beamTimer > 0f)
        {
            beamTimer -= Time.deltaTime;
            if (beamTimer <= 0f && lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }

        if (thirdPersonCamera == null || !thirdPersonCamera.IsAiming)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // 命中判定はレティクルと一致するカメラ基準で行う
        Transform camTransform = thirdPersonCamera.transform;
        Vector3 camOrigin = camTransform.position;
        Vector3 camDirection = camTransform.forward;
        Vector3 hitPoint = camOrigin + camDirection * maxDistance;

        if (Physics.Raycast(camOrigin, camDirection, out RaycastHit hit, maxDistance, ~0, QueryTriggerInteraction.Collide))
        {
            hitPoint = hit.point;

            Receiver receiver = hit.collider.GetComponent<Receiver>();
            if (receiver != null)
            {
                receiver.Activate();
            }
        }

        // 見た目のビームだけはプレイヤー側から命中点へ向けて描画する
        if (lineRenderer != null)
        {
            Vector3 beamOrigin = firePoint != null ? firePoint.position : transform.position + Vector3.up * 1.5f;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, beamOrigin);
            lineRenderer.SetPosition(1, hitPoint);
            beamTimer = beamDisplayDuration;
        }
    }
}
