using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f; // 単位/秒

    private readonly Queue<Vector3> pendingMoves = new Queue<Vector3>();
    private int nextWaypointIndex = 0;
    private bool isMoving = false;
    private bool isReturning = false;

    private Coroutine moveCoroutine;
    private Vector3 moveStartPosition;
    private Vector3 initialPosition;

    // 移動中、または移動待ちがまだ残っているか
    public bool IsBusy => isMoving || pendingMoves.Count > 0;

    // 他のキューブとの衝突で移動がキャンセルされたときに呼ばれる
    public event System.Action OnMoveBlocked;

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving && pendingMoves.Count > 0)
        {
            Vector3 destination = pendingMoves.Dequeue();
            moveStartPosition = transform.position;
            moveCoroutine = StartCoroutine(MoveToPoint(destination));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMoving || isReturning)
        {
            return;
        }

        if (other.CompareTag("Cube"))
        {
            CancelAndReturn();
            OnMoveBlocked?.Invoke();
        }
    }

    // 光線(Receiver)用: 次のウェイポイントへの移動をキューに積む
    public void TriggerMove()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        Transform target = waypoints[nextWaypointIndex];
        nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Length;

        if (target != null)
        {
            pendingMoves.Enqueue(target.position);
        }
    }

    // レバー用: シーケンスに関係なく、指定した番号の地点へ直接移動する
    public void MoveToWaypoint(int index)
    {
        if (waypoints == null || index < 0 || index >= waypoints.Length)
        {
            return;
        }

        Transform target = waypoints[index];
        if (target != null)
        {
            pendingMoves.Enqueue(target.position);
        }
    }

    // レバー用: シーン開始時の初期位置へ直接移動する
    public void MoveToInitialPosition()
    {
        pendingMoves.Enqueue(initialPosition);
    }

    // ボタン用: 今の移動を中断し、その移動を始める前の位置へ戻す
    public void CancelAndReturn()
    {
        pendingMoves.Clear();

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(ReturnToStart(moveStartPosition));
    }

    private IEnumerator MoveToPoint(Vector3 destination)
    {
        isMoving = true;

        Vector3 diff = destination - transform.position;
        Vector3 zStep = new Vector3(0f, 0f, diff.z); // 縦
        Vector3 xStep = new Vector3(diff.x, 0f, 0f); // 横
        Vector3 yStep = new Vector3(0f, diff.y, 0f); // 上下

        yield return MoveBy(zStep);
        yield return MoveBy(xStep);
        yield return MoveBy(yStep);

        transform.position = destination;
        isMoving = false;
    }

    // 他のキューブに衝突した際、来た経路を逆順にたどって元の位置へ戻る
    private IEnumerator ReturnToStart(Vector3 origin)
    {
        isMoving = true;
        isReturning = true;

        Vector3 diff = origin - transform.position;
        Vector3 yStep = new Vector3(0f, diff.y, 0f); // 上下
        Vector3 xStep = new Vector3(diff.x, 0f, 0f); // 横
        Vector3 zStep = new Vector3(0f, 0f, diff.z); // 縦

        yield return MoveBy(yStep);
        yield return MoveBy(xStep);
        yield return MoveBy(zStep);

        transform.position = origin;
        isReturning = false;
        isMoving = false;
    }

    private IEnumerator MoveBy(Vector3 delta)
    {
        if (delta.sqrMagnitude < 0.0001f)
        {
            yield break;
        }

        Vector3 start = transform.position;
        Vector3 target = start + delta;
        float duration = delta.magnitude / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        transform.position = target;
    }
}
