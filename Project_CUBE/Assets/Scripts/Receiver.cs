using UnityEngine;

// 光線が当たると、登録済みの全キューブに次の地点への移動を指示する
public class Receiver : MonoBehaviour
{
    [SerializeField] private CubeMove[] cubes;

    public void Activate()
    {
        if (cubes == null)
        {
            return;
        }

        foreach (CubeMove cube in cubes)
        {
            if (cube != null)
            {
                cube.TriggerMove();
            }
        }
    }
}
