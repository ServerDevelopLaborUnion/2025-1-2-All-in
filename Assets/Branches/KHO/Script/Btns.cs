using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btns : MonoBehaviour
{
    [SerializeField] private GameObject _signupPanel;
    [SerializeField] private GameObject Dead;
    [SerializeField] private SloltMachine SloltMachine;
    private RandomItem randomItem;

    private void Start()
    {

        if (_signupPanel != null)
        {
            _signupPanel.SetActive(false);
        }
        if (Dead != null)
        {
            Dead.SetActive(false);
        }
    }

    public void Qiut()
    {
        Application.Quit();
    }

    public void SignupOnOff()
    {
        if (!_signupPanel.activeSelf)
        {
            _signupPanel.SetActive(true);
        }
        else
        {
            _signupPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (MoneyManager.Instance != null && MoneyManager.Instance.Dead)
        {
            Dead.SetActive(true);
        }
        else return;
    }

    public void Reset()
    {
        MoneyManager.Instance.Dead = false;
        SceneManager.LoadScene("CTJMachine");
    }
}
