using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
