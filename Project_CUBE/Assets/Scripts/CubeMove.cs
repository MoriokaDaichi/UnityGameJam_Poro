using UnityEngine;

public class CubeMove : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 180f, 0f); // 度/秒

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
