using UnityEngine;

// レバーやボタンなど、プレイヤーが近づいたときに操作ヒントを表示したいオブジェクトに付ける
public class InteractionHint : MonoBehaviour
{
    [SerializeField] private string message = "Eキーで操作";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Show(this, message);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && InteractionHintUI.Instance != null)
        {
            InteractionHintUI.Instance.Hide(this);
        }
    }
}
