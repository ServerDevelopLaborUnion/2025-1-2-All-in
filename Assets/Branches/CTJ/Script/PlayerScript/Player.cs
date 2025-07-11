using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput _pi;
    PlayerMovement _pm;

    private void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        _pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        moveSet();
    }

    private void moveSet()
    {
        _pm.SetMove(_pi.moveDir.x, _pi.moveDir.y);
    }
}
