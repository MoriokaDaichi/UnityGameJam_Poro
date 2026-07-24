using UnityEngine;

// 光線が当たると、登録済みの全キューブに次の地点への移動を指示する
public class Receiver : MonoBehaviour
{
    [SerializeField] private CubeMove[] cubes;

    [SerializeField] private string separatedHintMessage = "キューブがレシーバーから外れています";

    public void Activate()
    {
        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            // 移動中(待ち行列含む)は追加の入力を受け付けない
            if (cube != null && !cube.IsBusy)
            {
                cube.TriggerMove();
            }
        }
    }

    // キューブタグのオブジェクトと重なった/外れたタイミングでヒントを出し分ける
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Hide(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cube") && InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Show(this, separatedHintMessage);
        }
    }
}
