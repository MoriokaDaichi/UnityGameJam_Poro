using UnityEngine;

public class reverController : MonoBehaviour
{
    [Header("プレイヤー設定")]
    public Transform playerTransform;
    public float interactDistance = 2.5f;

    [Header("レバーの設定")]
    [Tooltip("レバーの下にある、色がついたキューブ（土台など）をここにドラッグ＆ドロップ")]
    public GameObject baseCube;

    [Header("回転・移動の設定")]
    [Tooltip("手前に倒す角度（例: -45）")]
    public float angle1 = -45f;
    [Tooltip("奥に倒す角度（例: 45）")]
    public float angle2 = 45f;
    public float leverSpeed = 5f;
    public Vector3 blockMoveOffset = new Vector3(0f, 3f, 0f);
    public float blockSpeed = 3f;

    // 0: 初期(まっすぐ), 1: 状態1(手前), 2: 状態2(奥)
    private int leverState = 0;

    private Quaternion defaultRotation;
    private Quaternion rotation1;
    private Quaternion rotation2;

    private Transform targetBlock;
    private Vector3 blockInitialPos;
    private Vector3 blockTargetPos;

    // 外部（PuzzleManagerなど）からレバーがON（状態2＝動いた状態）かを確認するためのプロパティ
    public bool IsOn
    {
        get { return leverState == 2; } // 状態2（奥に倒れている時）をONとする
    }

    void Start()
    {
        // 1. Scene上に配置されているそのままの回転（まっすぐな状態）を記憶
        defaultRotation = transform.localRotation;

        // 2. 初期回転のEuler角を取得し、Z軸の回転だけをずらした目標角度を作成
        Vector3 defaultEuler = defaultRotation.eulerAngles;
        rotation1 = Quaternion.Euler(defaultEuler.x, defaultEuler.y, defaultEuler.z + angle1);
        rotation2 = Quaternion.Euler(defaultEuler.x, defaultEuler.y, defaultEuler.z + angle2);

        // 3. プレイヤーの自動検索
        if (playerTransform == null)
        {
            GameObject player = GameObject.Find("Cube");
            if (player != null) playerTransform = player.transform;
        }

        // 4. 下のキューブと同じ色のブロックを検索
        if (baseCube != null)
        {
            Renderer baseRend = baseCube.GetComponent<Renderer>();
            if (baseRend != null)
            {
                Color baseColor = baseRend.material.color;

                Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                foreach (Renderer rend in allRenderers)
                {
                    // 除外チェック
                    if (rend.gameObject == baseCube) continue;
                    if (rend.transform.IsChildOf(transform)) continue;
                    if (rend.transform.IsChildOf(baseCube.transform)) continue;

                    // 同色ブロックをターゲットとして登録
                    if (IsColorSimilar(rend.material.color, baseColor))
                    {
                        targetBlock = rend.transform;
                        blockInitialPos = targetBlock.localPosition;
                        blockTargetPos = blockInitialPos + blockMoveOffset;
                        Debug.Log(gameObject.name + " が動かすブロックを見つけました: " + targetBlock.name);
                        break;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 近づいてEキーを押したとき
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
        }

        // 状態に応じた目標回転を選択
        Quaternion targetLeverRot = defaultRotation;
        if (leverState == 1) targetLeverRot = rotation1;
        else if (leverState == 2) targetLeverRot = rotation2;

        // レバーの回転アニメーション（armが動く処理）
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLeverRot, Time.deltaTime * leverSpeed);

        // ブロックの移動アニメーション
        if (targetBlock != null)
        {
            Vector3 destBlockPos = (leverState == 2) ? blockTargetPos : blockInitialPos;
            targetBlock.localPosition = Vector3.Lerp(targetBlock.localPosition, destBlockPos, Time.deltaTime * blockSpeed);
        }
    }

    // レバーのON/OFF切り替え用関数
    public void ToggleLever()
    {
        if (leverState == 0 || leverState == 2)
        {
            leverState = 1;
        }
        else
        {
            leverState = 2;
        }
    }

    private bool IsColorSimilar(Color c1, Color c2)
    {
        float threshold = 0.1f;
        return Mathf.Abs(c1.r - c2.r) < threshold &&
               Mathf.Abs(c1.g - c2.g) < threshold &&
               Mathf.Abs(c1.b - c2.b) < threshold;
    }
}