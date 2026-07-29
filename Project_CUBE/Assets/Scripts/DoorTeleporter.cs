using UnityEngine;
using UnityEngine.InputSystem;

// ドアに近づくとヒントを表示し、Eキーで画面フェード中にプレイヤーを目的地へ転送する
public class DoorTeleporter : MonoBehaviour
{
    [SerializeField] private string hintMessage = "Eキーで開ける";
    [SerializeField] private Transform player;
    [SerializeField] private Transform destination;

    private bool playerNear = false;
    private bool isTeleporting = false;

    void Update()
    {
        bool pressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
                       (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame);

        if (!isTeleporting && playerNear && pressed)
        {
            StartTeleport();
        }
    }

    private void StartTeleport()
    {
        isTeleporting = true;

        if (InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Hide(this);
        }

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOutIn(Teleport);
        }
        else
        {
            Teleport();
            isTeleporting = false;
        }
    }

    private void Teleport()
    {
        if (player != null && destination != null)
        {
            // CharacterControllerがある場合、無効化してから位置を書き換える
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            player.SetPositionAndRotation(destination.position, destination.rotation);

            if (controller != null)
            {
                controller.enabled = true;
            }
        }

        // CharacterControllerの無効化/有効化でトリガーの追跡が途切れ、OnTriggerExitが
        // 発生しないことがあるため、テレポート後は近接状態を明示的にリセットする
        playerNear = false;
        isTeleporting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (InteractionHintUI.Instance != null)
            {
                InteractionHintUI.Instance.Show(this, hintMessage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            if (InteractionHintUI.Instance != null)
            {
                InteractionHintUI.Instance.Hide(this);
            }
        }
    }
}
