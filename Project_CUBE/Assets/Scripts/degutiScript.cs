using Unity.VisualScripting;
using UnityEngine;

public class degutiScript : MonoBehaviour
{
    [Header("�����̃��o�[�i4�j")]
    [SerializeField] private reverController[] correctLevers; // �������o�[�̃X�N���v�g

    [Header("����������Q���i���F�u���b�N�j")]
    [SerializeField] private GameObject doorBlock;

    private bool isCleared = false;

    void Update()
    {
        // ���ɃN���A�ς݂Ȃ画�肵�Ȃ�
        if (isCleared) return;

        // �N���A�����̃`�F�b�N
        if (CheckAllLeversOn())
        {
            ClearPuzzle();
        }
    }

    // ���ׂĂ̐������o�[��ON�iT����ԁj������
    private bool CheckAllLeversOn()
    {
        if (correctLevers == null || correctLevers.Length == 0) return false;

        foreach (var lever in correctLevers)
        {
            // ���o�[��isOn�v���p�e�B���Q�Ɓi�������ϐ���������Ă���΍��킹�Ă��������j
            if (lever == null || !lever.IsOn)
            {
                return false; // 1�ł�OFF������΃N���A����Ȃ�
            }
        }
        return true; // �S��ON�I
    }

    // �N���A���̏���
    private void ClearPuzzle()
    {
        isCleared = true;
        Debug.Log("�p�Y���N���A�I");

        if (doorBlock != null)
        {
            doorBlock.SetActive(false); // ���F�u���b�N������
        }
    }
}