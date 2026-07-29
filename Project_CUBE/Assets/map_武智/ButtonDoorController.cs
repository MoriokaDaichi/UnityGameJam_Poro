using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDoorController : MonoBehaviour
{
    [Header("ボタンの設定")]
    public Transform buttonTop;

    // 小さいほど少しだけ沈む
    public float pressDistance = 0.01f;

    public float pressSpeed = 5f;

    [Header("紫の扉")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("扉の設定")]
    public float openDistance = 5f;
    public float doorSpeed = 2f;

    private bool playerNear = false;
    private bool doorOpen = false;
    private bool wasHoldingButton = false;

    private Vector3 buttonStartPosition;
    private Vector3 buttonPressedPosition;

    private Vector3 leftStartPosition;
    private Vector3 rightStartPosition;

    private Vector3 leftOpenPosition;
    private Vector3 rightOpenPosition;

    void Start()
    {
        // =====================================
        // ボタン
        // =====================================
        if (buttonTop != null)
        {
            buttonStartPosition =
                buttonTop.localPosition;

            // 奥に少しだけ沈む
            buttonPressedPosition =
                buttonStartPosition +
                Vector3.forward * pressDistance;
        }

        // =====================================
        // 左の紫扉
        // =====================================
        if (leftDoor != null)
        {
            leftStartPosition =
                leftDoor.localPosition;

            leftOpenPosition =
                leftStartPosition +
                Vector3.back * openDistance;
        }

        // =====================================
        // 右の紫扉
        // =====================================
        if (rightDoor != null)
        {
            rightStartPosition =
                rightDoor.localPosition;

            rightOpenPosition =
                rightStartPosition +
                Vector3.forward * openDistance;
        }
    }

    void Update()
    {
        // =====================================
        // Eを押しているか(ゲームパッドXボタンも可)
        // =====================================
        bool eHeld = (Keyboard.current != null && Keyboard.current.eKey.isPressed) ||
                     (Gamepad.current != null && Gamepad.current.buttonWest.isPressed);

        bool holdingButton =
            playerNear &&
            eHeld;

        if (holdingButton && !wasHoldingButton && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Button");
        }
        wasHoldingButton = holdingButton;


        // =====================================
        // Eを押している間
        // ボタンを沈ませる
        // =====================================
        if (buttonTop != null)
        {
            Vector3 target;

            if (holdingButton)
            {
                target = buttonPressedPosition;
            }
            else
            {
                target = buttonStartPosition;
            }

            buttonTop.localPosition =
                Vector3.MoveTowards(
                    buttonTop.localPosition,
                    target,
                    pressSpeed * Time.deltaTime
                );
        }


        // =====================================
        // Eを押したら扉を開く
        // =====================================
        if (holdingButton)
        {
            doorOpen = true;
        }


        // =====================================
        // 左の紫扉
        // =====================================
        if (leftDoor != null)
        {
            Vector3 target =
                doorOpen
                ? leftOpenPosition
                : leftStartPosition;

            leftDoor.localPosition =
                Vector3.MoveTowards(
                    leftDoor.localPosition,
                    target,
                    doorSpeed * Time.deltaTime
                );
        }


        // =====================================
        // 右の紫扉
        // =====================================
        if (rightDoor != null)
        {
            Vector3 target =
                doorOpen
                ? rightOpenPosition
                : rightStartPosition;

            rightDoor.localPosition =
                Vector3.MoveTowards(
                    rightDoor.localPosition,
                    target,
                    doorSpeed * Time.deltaTime
                );
        }
    }


    // =========================================
    // リスポーン時に扉を閉じる
    // =========================================
    public void CloseDoor()
    {
        doorOpen = false;

        Debug.Log("紫の扉を閉じる！");
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