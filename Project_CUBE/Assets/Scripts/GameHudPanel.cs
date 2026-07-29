using UnityEngine;
using TMPro;

// 画面右上のパネルに、現在のステージに応じたヒントと残りステージ数を表示する
public class GameHudPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private TMP_Text stageCountText;

    [Tooltip("index 0 = 1ステージ目のヒント、index 1 = 2ステージ目のヒント...")]
    [TextArea]
    [SerializeField] private string[] stageHints;

    [SerializeField] private string stageCountFormat = "残り {0}/{1} ステージ";

    private int lastShownStageIndex = -1;

    void Update()
    {
        UpdateHint();
        UpdateStageCount();
    }

    private void UpdateHint()
    {
        if (hintText == null || StageManager.Instance == null || stageHints == null || stageHints.Length == 0)
        {
            return;
        }

        int stageIndex = Mathf.Clamp(StageManager.Instance.ClearedStages, 0, stageHints.Length - 1);

        if (stageIndex == lastShownStageIndex)
        {
            return;
        }

        lastShownStageIndex = stageIndex;
        hintText.text = stageHints[stageIndex];
    }

    private void UpdateStageCount()
    {
        if (stageCountText == null || StageManager.Instance == null)
        {
            return;
        }

        stageCountText.text = string.Format(stageCountFormat, StageManager.Instance.RemainingStages, StageManager.Instance.TotalStages);
    }
}
