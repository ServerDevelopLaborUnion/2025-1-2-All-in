using UnityEngine;
using UnityEngine.InputSystem;

public class SelectRaycast : MonoBehaviour
{
    Vector2 _mousePosition;
    Vector2 _worldmousePosition;

    SelectChecker _currentObject;
    SelectChecker _lastObject;
    Transform _player;

    [SerializeField] float selectRange = 5f;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Ray();
    }

    public void Ray()
    {
        _mousePosition = Mouse.current.position.value;
        _worldmousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(_worldmousePosition, Vector2.zero, Mathf.Infinity);

        if (hit)
        {
            if (hit.collider && hit.collider.gameObject.CompareTag("Target"))
            {
                float distance = Vector2.Distance(_player.position, hit.collider.transform.position);
                if (distance <= selectRange)
                {
                    _currentObject = hit.collider.gameObject.GetComponent<SelectChecker>();
                    _currentObject.GetActive();
                }
                else
                {
                    _currentObject.ExitActive();
                }

                if (_lastObject != _currentObject)
                {
                    RollbackTheLastObject();
                    _lastObject = _currentObject;
                }

            }
            else
            {
                RollbackTheLastObject();
            }
        }
    }

    private void RollbackTheLastObject()
    {
        if (_lastObject != null)
        {
            _lastObject.ExitActive();
        }
    }
}
