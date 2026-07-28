using UnityEngine;

public class StageClearWall : MonoBehaviour
{
    [Header("赤と青のクリア判定")]
    public CubeDoorTrigger redTrigger;
    public CubeDoorTrigger blueTrigger;

    [Header("上げる壁")]
    public Transform clearWall;

    public float upDistance = 10f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 upPosition;

    private bool isOpen = false;

    void Start()
    {
        if (clearWall != null)
        {
            startPosition = clearWall.localPosition;

            upPosition =
                startPosition + Vector3.up * upDistance;
        }
    }

    void Update()
    {
        // 赤と青の両方をクリア
        if (!isOpen &&
            redTrigger != null &&
            blueTrigger != null &&
            redTrigger.IsCleared &&
            blueTrigger.IsCleared)
        {
            isOpen = true;

            Debug.Log("赤と青クリア！次の壁OPEN！");
        }

        if (isOpen && clearWall != null)
        {
            clearWall.localPosition =
                Vector3.MoveTowards(
                    clearWall.localPosition,
                    upPosition,
                    moveSpeed * Time.deltaTime
                );
        }
    }
}