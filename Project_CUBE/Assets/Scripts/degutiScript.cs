using Unity.VisualScripting;
using UnityEngine;

public class degutiScript : MonoBehaviour
{
    [Header("正解のレバー（4つ）")]
    [SerializeField] private reverController[] correctLevers; // 正解レバーのスクリプト

    [Header("消したい障害物（水色ブロック）")]
    [SerializeField] private GameObject doorBlock;

    private bool isCleared = false;

    void Update()
    {
        // 既にクリア済みなら判定しない
        if (isCleared) return;

        // クリア条件のチェック
        if (CheckAllLeversOn())
        {
            ClearPuzzle();
        }
    }

    // すべての正解レバーがON（T字状態）か判定
    private bool CheckAllLeversOn()
    {
        if (correctLevers == null || correctLevers.Length == 0) return false;

        foreach (var lever in correctLevers)
        {
            // レバーのisOnプロパティを参照（※もし変数名が違っていれば合わせてください）
            if (lever == null || !lever.IsOn)
            {
                return false; // 1つでもOFFがあればクリアじゃない
            }
        }
        return true; // 全部ON！
    }

    // クリア時の処理
    private void ClearPuzzle()
    {
        isCleared = true;
        Debug.Log("パズルクリア！");

        if (doorBlock != null)
        {
            doorBlock.SetActive(false); // 水色ブロックを消す
        }
    }
}