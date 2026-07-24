using System.Collections.Generic;
using UnityEngine;

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
