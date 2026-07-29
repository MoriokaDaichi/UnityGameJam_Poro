using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 画面下部にインタラクションのヒントテキストを表示する管理役。シーンに1つ配置する。
public class InteractionHintUI : MonoBehaviour
{
    public static InteractionHintUI Instance { get; private set; }

    [SerializeField] private int fontSize = 24;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float bottomMargin = 100f;

    // 表示元(InteractionHintに限らず任意のコンポーネント)をキーにメッセージを管理する
    private readonly List<object> activeKeys = new List<object>();
    private readonly Dictionary<object, string> messages = new Dictionary<object, string>();

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    // シーン遷移をまたいで表示中のヒントが残らないよう、シーン読み込みのたびにクリアする
    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        activeKeys.Clear();
        messages.Clear();
    }

    public void Show(object key, string message)
    {
        if (!activeKeys.Contains(key))
        {
            activeKeys.Add(key);
        }
        messages[key] = message;
    }

    public void Hide(object key)
    {
        activeKeys.Remove(key);
        messages.Remove(key);
    }

    // リスポーン通知など、範囲判定を持たない一時的なメッセージ表示用
    public void ShowTemporary(object key, string message, float duration)
    {
        Show(key, message);
        StartCoroutine(HideAfterDelay(key, duration));
    }

    private IEnumerator HideAfterDelay(object key, float duration)
    {
        yield return new WaitForSeconds(duration);
        Hide(key);
    }

    void OnGUI()
    {
        if (activeKeys.Count == 0)
        {
            return;
        }

        // 一番最近表示要求があったものを表示する
        string message = messages[activeKeys[activeKeys.Count - 1]];
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = fontSize,
            alignment = TextAnchor.MiddleCenter,
            clipping = TextClipping.Overflow
        };
        style.normal.textColor = textColor;

        float width = 700f;
        float height = fontSize * 2f;
        Rect rect = new Rect((Screen.width - width) * 0.5f, Screen.height - bottomMargin - height, width, height);
        GUI.Label(rect, message, style);
    }
}
