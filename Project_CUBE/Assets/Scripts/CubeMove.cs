using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f; // 単位/秒

    private readonly Queue<Transform> pendingMoves = new Queue<Transform>();
    private int nextWaypointIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving && pendingMoves.Count > 0)
        {
            Transform target = pendingMoves.Dequeue();
            StartCoroutine(MoveToPoint(target.position));
        }
    }

    // Receiverから呼ばれ、次のウェイポイントへの移動をキューに積む
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
            pendingMoves.Enqueue(target);
        }
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
