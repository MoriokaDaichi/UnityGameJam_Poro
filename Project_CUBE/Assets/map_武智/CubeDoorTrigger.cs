using UnityEngine;

public class CubeDoorTrigger : MonoBehaviour
{
    // StageClearWallから確認するためのクリア判定
    public bool IsCleared { get; private set; } = false;

    [Header("開く扉")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("扉の設定")]
    public float openAngle = 90f;
    public float doorSpeed = 2.0f;

    // 扉の最初の角度
    private Quaternion leftStartRotation;
    private Quaternion rightStartRotation;

    // 扉が開いたときの角度
    private Quaternion leftOpenRotation;
    private Quaternion rightOpenRotation;

    // 扉を開くか
    private bool doorOpen = false;

    void Start()
    {
        // 扉が設定されているか確認
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("Left Door または Right Door が設定されていません！");
            return;
        }

        // 最初の角度を保存
        leftStartRotation = leftDoor.localRotation;
        rightStartRotation = rightDoor.localRotation;

        // 左扉は +90度
        leftOpenRotation =
            leftStartRotation *
            Quaternion.Euler(0f, -openAngle, 0f);

        // 右扉は -90度
        rightOpenRotation =
            rightStartRotation *
            Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        // まだクリアしていなければ何もしない
        if (!doorOpen)
        {
            return;
        }

        // 左扉を開く
        if (leftDoor != null)
        {
            leftDoor.localRotation =
                Quaternion.Slerp(
                    leftDoor.localRotation,
                    leftOpenRotation,
                    Time.deltaTime * doorSpeed
                );
        }

        // 右扉を開く
        if (rightDoor != null)
        {
            rightDoor.localRotation =
                Quaternion.Slerp(
                    rightDoor.localRotation,
                    rightOpenRotation,
                    Time.deltaTime * doorSpeed
                );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggerに入った: " + other.gameObject.name);

        // MoveCubeタグのキューブが入った
        if (other.CompareTag("MoveCube"))
        {
            // すでにクリア済みなら何もしない
            if (IsCleared)
            {
                return;
            }

            doorOpen = true;
            IsCleared = true;

            Debug.Log("ギミッククリア！扉OPEN！");
        }
    }
}