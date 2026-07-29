using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Main";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void Update()
    {
        // Spaceキー
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Spaceキーが押された");
            StartGame();
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Aボタンが押された");
            StartGame();
        }
    }
}
