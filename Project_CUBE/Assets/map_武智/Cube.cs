using UnityEngine;
using UnityEngine.InputSystem;

public class Cube : MonoBehaviour
{
    // =========================================
    // ボタン
    // =========================================

    [Header("このキューブを動かすボタン")]
    public Button button;


    // =========================================
    // 移動設定
    // =========================================

    [Header("移動速度")]
    public float moveSpeed = 2.0f;

    [Header("上に移動する距離")]
    public float upDistance = 3.0f;

    [Header("上がった後に移動する距離")]
    public float moveDistance = 4.0f;


    // =========================================
    // 移動方向
    // =========================================

    public enum MoveDirection
    {
        Blue,    // 前
        Red       // 右
    }

    [Header("上がった後の方向")]
    public MoveDirection moveDirection;


    // =========================================
    // 内部変数
    // =========================================

    private Vector3 startPosition;
    private Vector3 upPosition;
    private Vector3 movePosition;

    private bool reachedTop = false;


    // =========================================
    // Start
    // =========================================

    void Start()
    {
        // 最初の位置を保存
        startPosition = transform.localPosition;


        // 上に移動した位置
        upPosition =
            startPosition +
            Vector3.up * upDistance;


        // =====================================
        // 上がった後の移動先
        // =====================================

        if (moveDirection == MoveDirection.Blue)
        {
            // 青：プレイヤーから見て左
            movePosition =
                upPosition +
                Vector3.forward * moveDistance;
        }
        else
        {
            // 赤：プレイヤーから見て右
            movePosition =
                upPosition +
                Vector3.back * moveDistance;
        }
        //if (moveDirection == MoveDirection.Forward)
        //{
        //    // 青キューブ：前
        //    movePosition =
        //        upPosition +
        //        Vector3.forward * moveDistance;
        //}
        //else
        //{
        //    // 赤キューブ：右
        //    movePosition =
        //        upPosition +
        //        Vector3.left * moveDistance;
        //}
    }


    // =========================================
    // Update
    // =========================================

    void Update()
    {
        if (button == null)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }


        // =====================================
        // Eキーを押し続けている
        // =====================================

        if (button.PlayerNear &&
            Keyboard.current.eKey.isPressed)
        {
            // ---------------------------------
            // ① まず上へ
            // ---------------------------------

            if (!reachedTop)
            {
                transform.localPosition =
                    Vector3.MoveTowards(
                        transform.localPosition,
                        upPosition,
                        moveSpeed * Time.deltaTime
                    );


                // 上まで到着したか
                if (Vector3.Distance(
                    transform.localPosition,
                    upPosition) < 0.01f)
                {
                    transform.localPosition =
                        upPosition;

                    reachedTop = true;
                }
            }

            // ---------------------------------
            // ② 上まで行ったら指定方向へ
            // ---------------------------------

            else
            {
                transform.localPosition =
                    Vector3.MoveTowards(
                        transform.localPosition,
                        movePosition,
                        moveSpeed * Time.deltaTime
                    );
            }
        }


        // =====================================
        // Eキーを離した
        // =====================================

        else
        {
            // ---------------------------------
            // ① まず横方向を戻す
            // ---------------------------------

            if (reachedTop)
            {
                transform.localPosition =
                    Vector3.MoveTowards(
                        transform.localPosition,
                        upPosition,
                        moveSpeed * Time.deltaTime
                    );


                // 上の位置まで戻った
                if (Vector3.Distance(
                    transform.localPosition,
                    upPosition) < 0.01f)
                {
                    transform.localPosition =
                        upPosition;

                    reachedTop = false;
                }
            }

            // ---------------------------------
            // ② そのあと下へ戻す
            // ---------------------------------

            else
            {
                transform.localPosition =
                    Vector3.MoveTowards(
                        transform.localPosition,
                        startPosition,
                        moveSpeed * Time.deltaTime
                    );


                if (Vector3.Distance(
                    transform.localPosition,
                    startPosition) < 0.01f)
                {
                    transform.localPosition =
                        startPosition;
                }
            }
        }
    }
}