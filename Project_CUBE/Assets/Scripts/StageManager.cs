using UnityEngine;

// 全4ステージの進行状況を管理する。シーンをまたいで参照できるよう永続化する。
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private int totalStages = 4;

    private int clearedStages = 0;

    public int TotalStages => totalStages;
    public int ClearedStages => clearedStages;
    public int RemainingStages => Mathf.Max(totalStages - clearedStages, 0);

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

    // ステージ(パズル)が1つクリアされるたびに呼ぶ
    public void ClearStage()
    {
        clearedStages = Mathf.Min(clearedStages + 1, totalStages);
    }

    public void ResetProgress()
    {
        clearedStages = 0;
    }
}
