//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.EventSystems;

//public class Title : MonoBehaviour
//{
//    public GameObject gameStartButton;

//    void Start()
//    {
//        // 最初からGAME STARTを選択状態にする
//        EventSystem.current.SetSelectedGameObject(gameStartButton);
//    }

//    public void GameStart()
//    {
//        SceneManager.LoadScene("GameScene");
//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{

    public void StartGame()

    {

        SceneManager.LoadScene("GameScene");

    }


    void Update()

    {

        // Spaceキーif (Keyboard.current.spaceKey.wasPressedThisFrame)

        {

            Debug.Log("Spaceキーが押された");

            StartGame();

        }


        // コントローラー Aボタン（PSなら×）if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)

        {

            Debug.Log("Aボタンが押された");

            StartGame();

        }

    }

}