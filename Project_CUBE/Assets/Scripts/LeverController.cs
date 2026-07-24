using UnityEngine;
using UnityEngine.InputSystem;

public class LeverController : MonoBehaviour
{
    [Header("回転の設定")]
    [Tooltip("1つ目の倒れる角度（例: -45度）")]
    public float angle1 = -45f;

    [Tooltip("2つ目（反対側）の倒れる角度（例: 45度）")]
    public float angle2 = 45f;

    [Tooltip("アニメーションの滑らかさ")]
    public float smoothSpeed = 5f;

    // レバーを倒したときに動かすキューブ
    [SerializeField] private CubeMove[] cubes;

    private bool isState2 = false;     // false = 1つ目の角度(-45), true = 2つ目の角度(+45)
    private Quaternion rotation1;      // -45度の角度
    private Quaternion rotation2;      // +45度の角度
    private bool playerNear = false;   // プレイヤーが近くにいるか

    void Start()
    {
        // ゲーム開始時の角度を基準にして、両側の目標角度を計算
        Quaternion baseRotation = transform.localRotation;
        rotation1 = baseRotation * Quaternion.Euler(0f, 0f, angle1);
        rotation2 = baseRotation * Quaternion.Euler(0f, 0f, angle2);

        if (cubes != null)
        {
            foreach (CubeMove cube in cubes)
            {
                if (cube != null)
                {
                    cube.OnMoveBlocked += HandleCubeMoveBlocked;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (cubes != null)
        {
            foreach (CubeMove cube in cubes)
            {
                if (cube != null)
                {
                    cube.OnMoveBlocked -= HandleCubeMoveBlocked;
                }
            }
        }
    }

    // 連携キューブの移動が他ブロックとの衝突でキャンセルされたら、レバーの向きも元に戻す
    private void HandleCubeMoveBlocked()
    {
        isState2 = !isState2;
    }

    void Update()
    {
        // プレイヤーが近くにいる時だけ、Eキーを押すたびに状態を反転（切り替え）
        if (playerNear && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isState2 = !isState2;
            NotifyCubes();
        }

        // 状態に合わせて目標の角度を選ぶ
        Quaternion targetRotation = isState2 ? rotation2 : rotation1;

        // 目標の角度に向かって滑らかに回転させる
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // レバーの状態に応じて、キューブを「初期位置」と「waypoints先頭」のどちらかへ直接動かす
    private void NotifyCubes()
    {
        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube == null)
            {
                continue;
            }

            if (isState2)
            {
                cube.MoveToWaypoint(0);
            }
            else
            {
                cube.MoveToInitialPosition();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
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