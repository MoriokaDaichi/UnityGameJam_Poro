using UnityEngine;

public class WallRespawn : MonoBehaviour
{
    [Header("この壁を動かしているレバー")]
    public WallLeverController wallLever;

    // 壁が戻ってる途中ならtrue
    public bool CanRespawn()
    {
        if (wallLever == null)
        {
            return false;
        }

        return wallLever.IsReturning;
    }
}