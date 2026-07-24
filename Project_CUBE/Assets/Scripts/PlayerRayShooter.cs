using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// エイム中(右クリック)に左クリックでカメラ正面へ光線を発射し、Receiverに命中させる
public class PlayerRayShooter : MonoBehaviour
{
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private LineRenderer lineRendererPrefab; // 発射のたびにこれを複製する
    [SerializeField] private Transform firePoint; // 未設定ならプレイヤー自身の少し上から発射
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float beamSpeed = 30f; // 光線が飛ぶ速度(単位/秒)
    [SerializeField] private float beamVisualLength = 2.5f; // 飛んでいく光の帯の長さ
    [SerializeField] private float beamDisplayDuration = 0.15f; // 命中後、残光を表示する時間

    void Update()
    {
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
        Receiver receiver = null;

        if (Physics.Raycast(camOrigin, camDirection, out RaycastHit hit, maxDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;
            receiver = hit.collider.GetComponent<Receiver>();
        }

        if (lineRendererPrefab == null)
        {
            if (receiver != null)
            {
                receiver.Activate();
            }
            return;
        }

        Vector3 beamOrigin = firePoint != null ? firePoint.position : transform.position + Vector3.up * 1.5f;

        // 発射ごとに専用のLineRendererを生成するため、前の光線を消さずに何本も同時表示できる
        LineRenderer beam = Instantiate(lineRendererPrefab);
        beam.positionCount = 2;
        StartCoroutine(TravelBeam(beam, beamOrigin, hitPoint, receiver));
    }

    private IEnumerator TravelBeam(LineRenderer beam, Vector3 beamOrigin, Vector3 hitPoint, Receiver receiver)
    {
        beam.enabled = true;

        Vector3 toTarget = hitPoint - beamOrigin;
        float totalDistance = toTarget.magnitude;
        Vector3 direction = totalDistance > 0.0001f ? toTarget / totalDistance : Vector3.forward;
        float duration = totalDistance / beamSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float headDistance = Mathf.Min(elapsed / duration * totalDistance, totalDistance);
            float tailDistance = Mathf.Max(headDistance - beamVisualLength, 0f);

            beam.SetPosition(0, beamOrigin + direction * tailDistance);
            beam.SetPosition(1, beamOrigin + direction * headDistance);
            yield return null;
        }

        if (receiver != null)
        {
            receiver.Activate();
        }

        // 着弾後、短い残光を見せてから消す
        beam.SetPosition(0, hitPoint - direction * beamVisualLength);
        beam.SetPosition(1, hitPoint);

        yield return new WaitForSeconds(beamDisplayDuration);

        Destroy(beam.gameObject);
    }
}
