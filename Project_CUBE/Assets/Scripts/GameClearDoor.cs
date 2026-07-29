using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ドアに近づくとヒントを表示し、Eキーで画面フェード中にGameClearシーンへ遷移する
public class GameClearDoor : MonoBehaviour
{
    [SerializeField] private string hintMessage = "Eキーでクリア";
    [SerializeField] private string clearSceneName = "GameClear";

    private bool playerNear = false;
    private bool isTransitioning = false;

    void Update()
    {
        bool pressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
                       (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame);

        if (!isTransitioning && playerNear && pressed)
        {
            StartClear();
        }
    }

    private void StartClear()
    {
        if (!Application.CanStreamedLevelBeLoaded(clearSceneName))
        {
            Debug.LogError($"シーン '{clearSceneName}' がBuild Settingsに登録されていません。File > Build Profiles(Build Settings)から追加してください。");
            return;
        }

        isTransitioning = true;

        if (InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Hide(this);
        }

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOutIn(LoadClearScene);
        }
        else
        {
            LoadClearScene();
        }
    }

    private void LoadClearScene()
    {
        SceneManager.LoadScene(clearSceneName);
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
