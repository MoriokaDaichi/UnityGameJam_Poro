using UnityEngine;

public class WallRespawn : MonoBehaviour
{
    [Header("この壁を動かしているレバー")]
    public WallLeverController wallLever;

    // 壁が戻っている間だけでなく、常にリスポーンさせる
    public bool CanRespawn()
    {
        return true;
    }
}