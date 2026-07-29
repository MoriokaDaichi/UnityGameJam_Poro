using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// BGM(シーンごとの自動再生・クロスフェード)とSE(単発効果音)をまとめて管理する。シーンに1つ配置する。
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Serializable]
    public class SceneBgm
    {
        public string sceneName;
        public AudioClip clip;
    }

    [Header("シーンごとのBGM")]
    [SerializeField] private SceneBgm[] sceneBgms;
    [SerializeField] private float bgmVolume = 0.5f;
    [SerializeField] private float bgmFadeDuration = 1f;

    [Serializable]
    public class NamedClip
    {
        public string name;
        public AudioClip clip;
    }

    [Header("効果音")]
    [SerializeField] private float sfxVolume = 1f;
    [Tooltip("各スクリプトから名前で呼び出せるSE一覧。ここに1回登録すれば、個別のコンポーネントへの設定は不要")]
    [SerializeField] private NamedClip[] sfxClips;

    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void Start()
    {
        PlayBgmForScene(SceneManager.GetActiveScene().name);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBgmForScene(scene.name);
    }

    private void PlayBgmForScene(string sceneName)
    {
        AudioClip clip = null;

        if (sceneBgms != null)
        {
            foreach (SceneBgm entry in sceneBgms)
            {
                if (entry != null && entry.sceneName == sceneName)
                {
                    clip = entry.clip;
                    break;
                }
            }
        }

        if (clip == bgmSource.clip && bgmSource.isPlaying)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(CrossfadeBgm(clip));
    }

    private IEnumerator CrossfadeBgm(AudioClip nextClip)
    {
        // ポーズ中(Time.timeScale=0)でもフェードが止まらないようunscaledDeltaTimeを使う
        float startVolume = bgmSource.volume;
        float t = 0f;
        while (t < bgmFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, bgmFadeDuration > 0f ? t / bgmFadeDuration : 1f);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = nextClip;

        if (nextClip == null)
        {
            yield break;
        }

        bgmSource.Play();

        t = 0f;
        while (t < bgmFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0f, bgmVolume, bgmFadeDuration > 0f ? t / bgmFadeDuration : 1f);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // 名前を指定してSEを鳴らす(Sfx Clipsに登録した名前と一致するクリップを再生)
    public void PlaySFX(string clipName)
    {
        if (string.IsNullOrEmpty(clipName) || sfxClips == null)
        {
            return;
        }

        foreach (NamedClip entry in sfxClips)
        {
            if (entry != null && entry.name == clipName)
            {
                PlaySFX(entry.clip);
                return;
            }
        }
    }
}
