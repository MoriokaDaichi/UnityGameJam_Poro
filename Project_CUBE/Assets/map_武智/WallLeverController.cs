using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WallLeverController : MonoBehaviour
{
    // ==================================================
    // レバー
    // ==================================================

    [Header("レバーの動くパーツ")]
    public Transform[] leverParts;

    [Header("レバーの回転中心")]
    public Transform leverPivot;

    [Header("レバーの設定")]
    public float angle = -20f;

    [Header("レバーが倒れる速度")]
    public float leverSpeed = 100f;

    [Header("レバーが戻るまでの時間")]
    public float leverReturnWait = 1.0f;

    [Header("レバーが戻る速度")]
    public float leverReturnSpeed = 80f;

    [Header("レバーの回転軸")]
    public Vector3 leverRotationAxis = Vector3.right;


    // ==================================================
    // 壁
    // ==================================================

    [Header("順番に動かす壁")]
    public Transform[] walls;

    [Header("壁の移動距離")]
    public float moveDistance = 5.0f;

    [Header("壁が上がる速度")]
    public float upSpeed = 3.0f;

    [Header("壁が戻る速度")]
    public float returnSpeed = 1.0f;

    [Header("上がる時の壁同士の間隔")]
    public float upDelay = 1.0f;

    [Header("全部上がった後の待ち時間")]
    public float waitBeforeReturn = 2.0f;

    [Header("戻る時の壁同士の間隔")]
    public float returnDelay = 0.5f;


    // ==================================================
    // 内部変数
    // ==================================================

    private bool playerNear = false;
    private bool isOn = false;

    // 壁が戻っている途中か
    public bool IsReturning { get; private set; } = false;

    // 現在のレバー角度
    private float currentLeverAngle = 0f;

    // 壁の最初の位置
    private Vector3[] wallStartPositions;

    // 壁が上がった位置
    private Vector3[] wallTargetPositions;


    void Start()
    {
        // ==============================
        // 壁の位置を保存
        // ==============================

        wallStartPositions =
            new Vector3[walls.Length];

        wallTargetPositions =
            new Vector3[walls.Length];

        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == null)
            {
                continue;
            }

            wallStartPositions[i] =
                walls[i].localPosition;

            wallTargetPositions[i] =
                wallStartPositions[i] +
                Vector3.up * moveDistance;
        }
    }


    void Update()
    {
        // ==============================
        // Eキーで起動
        // ==============================

        if (!isOn &&
            playerNear &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOn = true;

            Debug.Log("壁レバーON！");

            // レバー演出
            StartCoroutine(LeverAnimation());

            // 壁演出
            StartCoroutine(WallAnimation());
        }
    }


    // ==================================================
    // レバー
    // 倒れる → 待つ → 戻る
    // ==================================================

    private IEnumerator LeverAnimation()
    {
        if (leverPivot == null)
        {
            Debug.LogWarning("Lever Pivotが設定されていません！");
            yield break;
        }

        // レバーを倒す
        while (Mathf.Abs(currentLeverAngle - angle) > 0.1f)
        {
            float newAngle =
                Mathf.MoveTowards(
                    currentLeverAngle,
                    angle,
                    leverSpeed * Time.deltaTime
                );

            float deltaAngle =
                newAngle - currentLeverAngle;

            RotateLever(deltaAngle);

            currentLeverAngle = newAngle;

            yield return null;
        }

        // 最後のズレを補正
        float finalDelta =
            angle - currentLeverAngle;

        RotateLever(finalDelta);

        currentLeverAngle = angle;

        // 少し待つ
        yield return new WaitForSeconds(
            leverReturnWait
        );

        // 元の角度へ戻す
        while (Mathf.Abs(currentLeverAngle) > 0.1f)
        {
            float newAngle =
                Mathf.MoveTowards(
                    currentLeverAngle,
                    0f,
                    leverReturnSpeed * Time.deltaTime
                );

            float deltaAngle =
                newAngle - currentLeverAngle;

            RotateLever(deltaAngle);

            currentLeverAngle = newAngle;

            yield return null;
        }

        // ピッタリ元に戻す
        float returnDelta =
            -currentLeverAngle;

        RotateLever(returnDelta);

        currentLeverAngle = 0f;
    }


    // ==================================================
    // レバーのパーツをまとめて回転
    // ==================================================

    private void RotateLever(float deltaAngle)
    {
        if (leverPivot == null)
        {
            return;
        }

        Vector3 axis =
            leverPivot.TransformDirection(
                leverRotationAxis.normalized
            );

        for (int i = 0; i < leverParts.Length; i++)
        {
            if (leverParts[i] == null)
            {
                continue;
            }

            leverParts[i].RotateAround(
                leverPivot.position,
                axis,
                deltaAngle
            );
        }
    }


    // ==================================================
    // 壁の演出
    // ==================================================

    private IEnumerator WallAnimation()
    {
        // ==============================
        // 手前 → 奥
        // ① → ② → ③ → ④
        // ==============================

        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == null)
            {
                continue;
            }

            // 上がり切るまで待つ
            yield return StartCoroutine(
                MoveWall(
                    walls[i],
                    wallTargetPositions[i],
                    upSpeed
                )
            );

            // 次の壁まで待つ
            yield return new WaitForSeconds(
                upDelay
            );
        }


        // ==============================
        // 全部上がった状態で待つ
        // ==============================

        yield return new WaitForSeconds(
            waitBeforeReturn
        );


        // ==============================
        // ここから壁が戻り始める
        // ==============================

        IsReturning = true;

        Debug.Log("壁の戻り開始！");


        // ==============================
        // 奥 → 手前
        // ④ → ③ → ② → ①
        // ==============================

        for (int i = walls.Length - 1; i >= 0; i--)
        {
            if (walls[i] == null)
            {
                continue;
            }

            // 戻り終わる前に次の壁も開始
            StartCoroutine(
                MoveWall(
                    walls[i],
                    wallStartPositions[i],
                    returnSpeed
                )
            );

            yield return new WaitForSeconds(
                returnDelay
            );
        }


        // ==============================
        // 全部の壁が戻るまで待つ
        // ==============================

        bool allReturned = false;

        while (!allReturned)
        {
            allReturned = true;

            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i] == null)
                {
                    continue;
                }

                if (Vector3.Distance(
                    walls[i].localPosition,
                    wallStartPositions[i]) > 0.01f)
                {
                    allReturned = false;
                    break;
                }
            }

            yield return null;
        }


        // ==============================
        // 壁の戻り終了
        // ==============================

        IsReturning = false;

        Debug.Log("壁の戻り終了！");

        // またEキーで押せるようにする
        isOn = false;

        Debug.Log("レバーがまた使用可能！");
    }


    // ==================================================
    // 壁1枚を動かす
    // ==================================================

    private IEnumerator MoveWall(
        Transform wall,
        Vector3 targetPosition,
        float speed)
    {
        while (Vector3.Distance(
            wall.localPosition,
            targetPosition) > 0.01f)
        {
            wall.localPosition =
                Vector3.MoveTowards(
                    wall.localPosition,
                    targetPosition,
                    speed * Time.deltaTime
                );

            yield return null;
        }

        wall.localPosition = targetPosition;
    }


    // ==================================================
    // プレイヤー判定
    // ==================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            Debug.Log("壁レバーの範囲に入った");
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