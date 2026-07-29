using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameClear : MonoBehaviour
{
    [Header("点滅するテキスト(Canvas上のTMP_Textを指定)")]
    [SerializeField] private TMP_Text blinkText;
    [SerializeField] private float blinkSpeed = 2f; // 点滅の速さ
    [SerializeField] private float minAlpha = 0.2f; // 最も薄くなるときの不透明度

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    void Update()
    {
        if (Keyboard.current != null &&
            (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame))
        {
            Debug.Log("Space/Enterキーが押された");
            BackToTitle();
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Aボタンが押された");
            BackToTitle();
        }

        if (blinkText == null)
        {
            return;
        }

        // サイン波で0~1を滑らかに往復させ、フェードするような点滅にする
        float wave = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
        blinkText.alpha = Mathf.Lerp(minAlpha, 1f, wave);
    }
}
