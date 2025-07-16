using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;

public class Spinwheel : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public void StartRotation()
    {
        _rb.AddTorque(30f);
    }
}
