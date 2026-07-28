using UnityEngine;

public class Risupon : MonoBehaviour
{
    [Header("復活地点")]
    public Transform respawnPoint;

    [Header("扉を動かしているボタン")]
    public ButtonDoorController buttonDoorController;

    private CharacterController controller;

    void Start()
    {
        // PlayerについているCharacterControllerを取得
        controller =
            GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError(
                "CharacterControllerがありません！"
            );
        }
    }

    // =========================================
    // リスポーン
    // =========================================
    public void ResetPosition()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning(
                "Respawn Pointが設定されていません！"
            );

            return;
        }

        if (controller == null)
        {
            return;
        }

        // CharacterControllerを一時停止
        controller.enabled = false;

        // 復活地点へ戻す
        transform.position =
            respawnPoint.position;

        transform.rotation =
            Quaternion.Euler(0f, 180f, 0f);

        // CharacterControllerを戻す
        controller.enabled = true;

        // 紫の扉を閉じる
        if (buttonDoorController != null)
        {
            buttonDoorController.CloseDoor();
        }

        Debug.Log(
            "プレイヤーを復活地点に戻しました！"
        );
    }

    // =========================================
    // CharacterControllerが壁にぶつかった
    // =========================================
    private void OnControllerColliderHit(
        ControllerColliderHit hit)
    {
        // 当たった物か、その親からWallRespawnを探す
        WallRespawn wallRespawn =
            hit.gameObject.GetComponentInParent<WallRespawn>();

        if (wallRespawn == null)
        {
            return;
        }

        // 戻っている途中じゃなければ何もしない
        if (!wallRespawn.CanRespawn())
        {
            return;
        }

        Debug.Log(
            "戻ってくる壁に当たった！リスポーン！"
        );

        ResetPosition();
    }
}