using System;
using System.Collections;
using UnityEngine;

// 画面全体を黒でフェードアウト→フェードインさせる管理役。シーンに1つ配置する。
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private float fadeDuration = 0.5f;

    private float alpha = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 完全に暗転したタイミングでonFullyFadedを実行し、その後フェードインする
    public void FadeOutIn(Action onFullyFaded)
    {
        StartCoroutine(FadeRoutine(onFullyFaded));
    }

    private IEnumerator FadeRoutine(Action onFullyFaded)
    {
        yield return Fade(0f, 1f);
        onFullyFaded?.Invoke();
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        alpha = to;
    }

    void OnGUI()
    {
        if (alpha <= 0f)
        {
            return;
        }

        Color previous = GUI.color;
        GUI.color = new Color(0f, 0f, 0f, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = previous;
    }
}
