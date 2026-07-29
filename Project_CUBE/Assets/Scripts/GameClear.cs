using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameClear : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    void Update()
    {
        // Spaceキー
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Spaceキーが押された");
            BackToTitle();
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Aボタンが押された");
            BackToTitle();
        }
    }
}
