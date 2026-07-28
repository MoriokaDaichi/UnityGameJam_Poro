using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // プレイヤーとの距離
    public float distance = 3.0f;

    // プレイヤーを見る高さ
    public float height = 1.0f;

    // マウス感度
    public float mouseSensitivity = 0.15f;

    private float yaw = 0f;
    private float pitch = 15f;

    void Start()
    {
        // マウスカーソルを画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // マウス入力
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;
        }

        // 上下を向きすぎないように制限
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        // マウスからカメラの回転を作る
        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        // プレイヤーを見る中心位置
        Vector3 targetPosition =
            target.position + Vector3.up * height;

        // カメラをプレイヤーの後ろに配置
        Vector3 cameraPosition =
            targetPosition
            - rotation * Vector3.forward * distance;

        transform.position = cameraPosition;

        // カメラの向き
        transform.rotation = rotation;
    }
}