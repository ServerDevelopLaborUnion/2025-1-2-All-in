using System.Collections;
using UnityEngine;

public class Spinwheel : MonoBehaviour
{
    public bool NowSpin { get;  private set; } = false;
    private Rigidbody2D _rb;
    private Coroutine _nowCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Debug.Log(_rb.angularVelocity);
    }

    public void StartRotation()
    {
        _rb.AddTorque(100f, ForceMode2D.Impulse);
        if (_nowCoroutine != null)
        {
            StopCoroutine(_nowCoroutine);
        }

        _nowCoroutine =  StartCoroutine(Torqueslow());
    }

    private IEnumerator Torqueslow()
    {
        while (true)
        {
            if (_rb.angularVelocity <= 5f)
            {
                break;
            }

            if (_rb.angularVelocity > 100f)
            {
                _rb.angularVelocity -= 100;
            }
            else if (_rb.angularVelocity < 100f)
            {
                _rb.angularVelocity -= 10;
            }
            else if (_rb.angularVelocity < 10f)
            {
                _rb.angularVelocity -= 1;
            }

            yield return new WaitForSeconds(0.1f);
        }
        _rb.angularVelocity = Mathf.Lerp(_rb.angularVelocity, 0, 3f);

        NowSpin = true;
    }
}
