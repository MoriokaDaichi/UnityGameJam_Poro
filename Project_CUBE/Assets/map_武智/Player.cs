using UnityEngine;
using UnityEngine.InputSystem;

//public class Player : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float gravity = -20f;
//    public float rotateSpeed = 10f;

//    [Header("カメラ")]
//    public Transform cameraTransform;

//    [Header("復活地点")]
//    public Transform respawnPoint;

//    [Header("扉を動かしているボタン")]
//    public ButtonDoorController buttonDoorController;

//    private CharacterController controller;
//    private float verticalSpeed;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    void Update()
//    {
//        Vector2 input = Vector2.zero;

//        if (Keyboard.current != null)
//        {
//            if (Keyboard.current.aKey.isPressed)
//                input.x = -1;

//            if (Keyboard.current.dKey.isPressed)
//                input.x = 1;

//            if (Keyboard.current.sKey.isPressed)
//                input.y = -1;

//            if (Keyboard.current.wKey.isPressed)
//                input.y = 1;
//        }

//        // カメラの前方向
//        Vector3 cameraForward = cameraTransform.forward;
//        cameraForward.y = 0f;
//        cameraForward.Normalize();

//        // カメラの右方向
//        Vector3 cameraRight = cameraTransform.right;
//        cameraRight.y = 0f;
//        cameraRight.Normalize();

//        // カメラ基準の移動方向
//        Vector3 horizontalMove =
//            cameraForward * input.y +
//            cameraRight * input.x;

//        horizontalMove = horizontalMove.normalized;

//        // 移動方向にプレイヤーを向ける
//        if (horizontalMove.sqrMagnitude > 0.01f)
//        {
//            Quaternion targetRotation =
//                Quaternion.LookRotation(horizontalMove);

//            transform.rotation =
//                Quaternion.Slerp(
//                    transform.rotation,
//                    targetRotation,
//                    rotateSpeed * Time.deltaTime
//                );
//        }

//        horizontalMove *= moveSpeed;

//        // 重力
//        if (controller.isGrounded &&
//            verticalSpeed < 0f)
//        {
//            verticalSpeed = -2f;
//        }

//        verticalSpeed += gravity * Time.deltaTime;

//        Vector3 move = horizontalMove;
//        move.y = verticalSpeed;

//        controller.Move(
//            move * Time.deltaTime
//        );
//    }

    //// =========================================
    //// リスポーン
    //// =========================================
    //public void ResetPosition()
    //{
    //    if (respawnPoint == null)
    //    {
    //        Debug.LogWarning(
    //            "Respawn Pointが設定されていません！"
    //        );

    //        return;
    //    }

    //    // CharacterControllerを一時停止
    //    controller.enabled = false;

    //    // 復活地点へ戻す
    //    transform.position =
    //        respawnPoint.position;

    //    transform.rotation =
    //        respawnPoint.rotation;

    //    // 落下速度をリセット
    //    verticalSpeed = 0f;

    //    controller.enabled = true;

    //    // =====================================
    //    // 扉を閉じる
    //    // =====================================
    //    if (buttonDoorController != null)
    //    {
    //        buttonDoorController.CloseDoor();
    //    }

    //    Debug.Log(
    //        "プレイヤーを復活地点に戻しました！"
    //    );
    //}

    //// =========================================
    //// CharacterControllerが何かにぶつかった
    //// =========================================
    //private void OnControllerColliderHit(
    //    ControllerColliderHit hit)
    //{
    //    WallRespawn wallRespawn =
    //        hit.gameObject.GetComponent<WallRespawn>();

    //    if (wallRespawn == null)
    //    {
    //        return;
    //    }

    //    // 壁が戻っている途中だけリスポーン
    //    if (wallRespawn.CanRespawn())
    //    {
    //        Debug.Log(
    //            "戻ってくる壁に当たった！"
    //        );

    //        ResetPosition();
    //    }
    //}
//}