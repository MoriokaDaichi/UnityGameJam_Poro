using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            direction.z = 1.0f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction.z = -1.0f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction.y = 90.0f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction.y = -90.0f;
        }
        transform.Translate(direction * Time.deltaTime);
        transform.Rotate(rotation * Time.deltaTime);
    }
}
