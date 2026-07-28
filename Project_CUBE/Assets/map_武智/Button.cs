using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    // 押すボタン
    public Transform button;

    // このボタンで壁を動かすか
    public bool moveWalls = false;

    // 上に動かす壁
    public Transform rightWall;
    public Transform frontWall;
    public Transform leftWall;

    // 壁が上がる高さ
    public float wallUpDistance = 5.0f;

    // 壁が上がる速さ
    public float wallSpeed = 2.0f;


    // CubeMoveから確認するため
    public bool PlayerNear
    {
        get { return playerNear; }
    }


    // ボタンの元の位置
    private Vector3 buttonStartPos;

    // 壁の元の位置
    private Vector3 rightWallStartPos;
    private Vector3 frontWallStartPos;
    private Vector3 leftWallStartPos;

    private bool playerNear = false;

    // 壁を動かすか
    private bool wallsMove = false;


    void Start()
    {
        // ボタンの最初の位置を保存
        if (button != null)
        {
            buttonStartPos = button.localPosition;
        }

        // 壁を動かすボタンだけ壁の位置を保存
        if (moveWalls)
        {
            if (rightWall != null)
            {
                rightWallStartPos = rightWall.localPosition;
            }

            if (frontWall != null)
            {
                frontWallStartPos = frontWall.localPosition;
            }

            if (leftWall != null)
            {
                leftWallStartPos = leftWall.localPosition;
            }
        }
    }


    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }


        // プレイヤーが近くでEキーを押している間
        if (playerNear && Keyboard.current.eKey.isPressed)
        {
            if (button != null)
            {
                button.localPosition =
                    buttonStartPos + new Vector3(0f, 0f, 0.03f);
            }
        }
        else
        {
            // Eキーを離したらボタンを元に戻す
            if (button != null)
            {
                button.localPosition = buttonStartPos;
            }
        }


        // Eキーを押した瞬間
        if (playerNear &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("ボタンを押した！");

            // このボタンが壁を動かす設定なら
            if (moveWalls)
            {
                wallsMove = true;
            }
        }


        // 壁を上に動かす
        if (wallsMove && moveWalls)
        {
            // 右の壁
            if (rightWall != null)
            {
                Vector3 rightTarget =
                    rightWallStartPos + Vector3.up * wallUpDistance;

                rightWall.localPosition = Vector3.MoveTowards(
                    rightWall.localPosition,
                    rightTarget,
                    wallSpeed * Time.deltaTime
                );
            }


            // 前の壁
            if (frontWall != null)
            {
                Vector3 frontTarget =
                    frontWallStartPos + Vector3.up * wallUpDistance;

                frontWall.localPosition = Vector3.MoveTowards(
                    frontWall.localPosition,
                    frontTarget,
                    wallSpeed * Time.deltaTime
                );
            }


            // 左の壁
            if (leftWall != null)
            {
                Vector3 leftTarget =
                    leftWallStartPos + Vector3.up * wallUpDistance;

                leftWall.localPosition = Vector3.MoveTowards(
                    leftWall.localPosition,
                    leftTarget,
                    wallSpeed * Time.deltaTime
                );
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            Debug.Log("プレイヤーがボタンに近づいた！");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}