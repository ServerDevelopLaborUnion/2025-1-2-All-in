using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using TMPro;

public class LogginBtn : MonoBehaviour
{
    [SerializeField] private TMP_InputField IDInputField;
    [SerializeField] private TMP_InputField PinInputField;

    private void Awake()
    {
        var bro = Backend.Initialize();
    }

    public void LogginAcppt()
    {
        BackEndLogin.Instance.Login(IDInputField.text.ToString(), PinInputField.text.ToString());
    }
    
}
