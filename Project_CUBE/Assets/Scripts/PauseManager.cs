using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string titleSceneName = "Title";

    private bool isPaused = false;

    public static bool IsPaused { get; private set; }

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        IsPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void Resume()
    {
        isPaused = false;
        IsPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void BackToTitle()
    {
        if (!Application.CanStreamedLevelBeLoaded(titleSceneName))
        {
            Debug.LogError($"シーン '{titleSceneName}' がBuild Settingsに登録されていません。File > Build Profiles(Build Settings)から追加してください。");
            return;
        }

        isPaused = false;
        IsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }
}
