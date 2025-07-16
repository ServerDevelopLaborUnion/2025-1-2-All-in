using System;
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

    [Header("Cultivation")]
    [SerializeField] LayerMask _farmlandLayer;
    [SerializeField] GameObject _growObj;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Ray();
        Click();
    }

    private void Click()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            RaycastHit2D cHit = Physics2D.Raycast(_worldmousePosition, Vector2.zero, Mathf.Infinity, _farmlandLayer);
            if (cHit.collider != null)
            {
                Farmland cHitLand = cHit.collider.gameObject.GetComponent<Farmland>();

                if (!cHitLand.IsGrowing)
                {
                    Vector2 spawnPos = cHit.collider.transform.position;
                    Instantiate(_growObj, spawnPos, Quaternion.identity);
                    cHitLand.IsGrowing = true;
                }
                return;
            }

            if (_currentObject != null && _currentObject.IsActive)
            {
                _currentObject.Mining();
            }
        }
    }

    public void Ray()
    {
        if (_currentObject != null && _currentObject.gameObject == null) _currentObject = null;
        if (_lastObject != null && _lastObject.gameObject == null) _lastObject = null;

        _mousePosition = Mouse.current.position.value;
        _worldmousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(_worldmousePosition, Vector2.zero, Mathf.Infinity);

        if (hit)
        {
            if (hit.collider && hit.collider.gameObject.CompareTag("Target"))
            {
                _currentObject = hit.collider.gameObject.GetComponent<SelectChecker>();
                float distance = Vector2.Distance(_player.position, hit.collider.transform.position);
                if (distance <= selectRange)
                {
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
        else
        {
            RollbackTheLastObject();
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
