using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rigid2d;
    [SerializeField] float _moveSpeed = 6f;

    public float _xMove;
    public float _yMove;

    private void Awake()
    {
        _rigid2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        _rigid2d.linearVelocityX = _xMove * _moveSpeed;
        _rigid2d.linearVelocityY = _yMove * _moveSpeed;
    }

    public void SetMove(float xMove, float yMove)
    {
        _xMove = xMove;
        _yMove = yMove;
    }
}
