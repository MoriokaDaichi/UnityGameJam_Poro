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

    private Quaternion rotation1;      // -45度の角度
    private Quaternion rotation2;      // +45度の角度
    private bool playerNear = false;   // プレイヤーが近くにいるか

    void Start()
    {
        // ゲーム開始時の角度を基準にして、両側の目標角度を計算
        Quaternion baseRotation = transform.localRotation;
        rotation1 = baseRotation * Quaternion.Euler(0f, 0f, angle1);
        rotation2 = baseRotation * Quaternion.Euler(0f, 0f, angle2);
    }

    void Update()
    {
        // プレイヤーが近くにいる時だけ、Eキー(またはゲームパッドXボタン)を押すたびにキューブへ「今いない方」への移動を指示
        bool pressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
                       (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame);

        if (playerNear && pressed)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Lever");
            }

            NotifyCubes();
        }

        // レバーの向きは、キューブが今どちらにいる(向かっている)かをそのまま反映する
        Quaternion targetRotation = IsAtWaypoint() ? rotation2 : rotation1;

        // 目標の角度に向かって滑らかに回転させる
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // 連携キューブ(先頭)がWaypoint側にいるかどうかを、レバーの向き決定に使う
    private bool IsAtWaypoint()
    {
        if (cubes == null)
        {
            return false;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube != null)
            {
                return cube.AtWaypoint;
            }
        }

        return false;
    }

    private void NotifyCubes()
    {
        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube != null)
            {
                cube.ToggleMove();
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
