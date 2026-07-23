using UnityEngine;

// エイム中(右クリック)のみ画面中央にレティクルを表示する
public class AimReticle : MonoBehaviour
{
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private float size = 6f;
    [SerializeField] private Color color = Color.white;

    void OnGUI()
    {
        if (thirdPersonCamera == null || !thirdPersonCamera.IsAiming)
        {
            return;
        }

        GUI.color = color;
        float x = (Screen.width - size) * 0.5f;
        float y = (Screen.height - size) * 0.5f;
        GUI.DrawTexture(new Rect(x, y, size, size), Texture2D.whiteTexture);
    }
}
