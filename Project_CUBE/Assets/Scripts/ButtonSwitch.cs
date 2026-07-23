using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonSwitch : MonoBehaviour
{
    // 沈ませるボタン
    public Transform button;

    // ボタンの元の位置
    private Vector3 startPos;

    // ボタンが動いているか
    private bool isMoving = false;

    // プレイヤーが近くにいるか
    private bool playerNear = false;

    void Start()
    {
        startPos = button.position;
    }

    void Update()
    {
        if (playerNear &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("ボタンを押した！");

            if (!isMoving)
            {
                StartCoroutine(PushButton());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }

    IEnumerator PushButton()
    {
        isMoving = true;

        // 少し沈む
        button.position = startPos + new Vector3(0f, 0f, 0.03f);

        yield return new WaitForSeconds(0.3f);

        // 元に戻る
        button.position = startPos;

        isMoving = false;
    }
}