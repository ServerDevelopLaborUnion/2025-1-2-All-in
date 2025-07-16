using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 moveDir { get; private set; }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
}
