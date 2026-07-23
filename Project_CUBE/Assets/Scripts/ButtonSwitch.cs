using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSwitch : MonoBehaviour
{
    // 沈ませるボタン
    public Transform button;

    // 押している間、動かすキューブ
    [SerializeField] private CubeMove[] cubes;

    // 沈み込む深さ
    [SerializeField] private float pressDepth = 0.03f;

    // ボタンが沈む/戻る速度(単位/秒)
    [SerializeField] private float pressSpeed = 0.1f;

    // ボタンの元の位置・押し込んだ位置
    private Vector3 startPos;
    private Vector3 pressedPos;

    // 現在押し込まれているか(長押し中か)
    private bool isHeld = false;

    // プレイヤーが近くにいるか
    private bool playerNear = false;

    void Start()
    {
        startPos = button.position;
        pressedPos = startPos + new Vector3(0f, 0f, pressDepth);
    }

    void Update()
    {
        UpdateHeldState();

        // 押している間だけpressedPos、離したらstartPosへ滑らかに追従
        Vector3 target = isHeld ? pressedPos : startPos;
        button.position = Vector3.MoveTowards(button.position, target, pressSpeed * Time.deltaTime);
    }

    private void UpdateHeldState()
    {
        if (playerNear && Keyboard.current != null)
        {
            if (!isHeld && Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartHold();
                return;
            }

            if (isHeld && Keyboard.current.eKey.wasReleasedThisFrame)
            {
                EndHold();
                return;
            }
        }
        else if (isHeld)
        {
            // 範囲外に出たら強制的に離した扱いにする
            EndHold();
        }
    }

    private void StartHold()
    {
        isHeld = true;

        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube != null)
            {
                cube.MoveToWaypoint(0);
            }
        }
    }

    private void EndHold()
    {
        isHeld = false;

        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube != null)
            {
                cube.CancelAndReturn();
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
