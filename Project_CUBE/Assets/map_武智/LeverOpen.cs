using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class LeverOpen : MonoBehaviour
{
    [Header("レバーの動くパーツ")]
    public Transform[] leverParts;

    [Header("レバーの回転中心")]
    public Transform leverPivot;

    [Header("レバーの設定")]
    public float angle = -20f;
    public float leverSpeed = 100f;
    public float returnWait = 1.0f;
    public float leverReturnSpeed = 80f;

    [Header("レバーの回転軸")]
    public Vector3 leverRotationAxis = Vector3.right;

    [Header("動かす壁")]
    public Transform wall;

    [Header("壁の設定")]
    public float wallUpDistance = 5f;
    public float wallSpeed = 2f;

    private bool isOn = false;
    private bool playerNear = false;

    private float currentLeverAngle = 0f;

    private Vector3 wallStartPosition;
    private Vector3 wallUpPosition;

    void Start()
    {
        // 壁の位置
        if (wall != null)
        {
            wallStartPosition = wall.localPosition;

            wallUpPosition =
                wallStartPosition +
                Vector3.up * wallUpDistance;
        }
    }

    void Update()
    {
        // 近くでEキー
        if (!isOn &&
            playerNear &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOn = true;

            Debug.Log("レバーON！");

            StartCoroutine(LeverAnimation());
        }

        // 壁を上げる
        if (isOn && wall != null)
        {
            wall.localPosition =
                Vector3.MoveTowards(
                    wall.localPosition,
                    wallUpPosition,
                    wallSpeed * Time.deltaTime
                );
        }
    }

    // =====================================
    // レバーを倒して戻す
    // =====================================

    private IEnumerator LeverAnimation()
    {
        if (leverPivot == null)
        {
            Debug.LogWarning("Lever Pivotが設定されていません！");
            yield break;
        }

        // -------------------------
        // 倒す
        // -------------------------

        while (Mathf.Abs(currentLeverAngle - angle) > 0.1f)
        {
            float nextAngle =
                Mathf.MoveTowards(
                    currentLeverAngle,
                    angle,
                    leverSpeed * Time.deltaTime
                );

            float deltaAngle =
                nextAngle - currentLeverAngle;

            RotateLever(deltaAngle);

            currentLeverAngle = nextAngle;

            yield return null;
        }

        // 最後のズレを補正
        float finalDelta =
            angle - currentLeverAngle;

        RotateLever(finalDelta);

        currentLeverAngle = angle;

        // 少し待つ
        yield return new WaitForSeconds(returnWait);

        // -------------------------
        // 元に戻す
        // -------------------------

        while (Mathf.Abs(currentLeverAngle) > 0.1f)
        {
            float nextAngle =
                Mathf.MoveTowards(
                    currentLeverAngle,
                    0f,
                    leverReturnSpeed * Time.deltaTime
                );

            float deltaAngle =
                nextAngle - currentLeverAngle;

            RotateLever(deltaAngle);

            currentLeverAngle = nextAngle;

            yield return null;
        }

        float returnDelta =
            -currentLeverAngle;

        RotateLever(returnDelta);

        currentLeverAngle = 0f;
    }

    // =====================================
    // レバーのパーツだけ回す
    // =====================================

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            Debug.Log("レバーの範囲に入った");
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