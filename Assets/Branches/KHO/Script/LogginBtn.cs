using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogginBtn : MonoBehaviour
{
    [SerializeField] private TMP_InputField IDInputField;
    [SerializeField] private TMP_InputField PinInputField;
    [SerializeField] private TMP_InputField SginUpIDInputField;
    [SerializeField] private TMP_InputField SginUpPinInputInputField;
    [SerializeField] private TMP_InputField NickNameInputInputField;
    [SerializeField] private GameObject NickNameSetWin;
    [SerializeField] private GameObject ChageNameFail;
    [SerializeField] private GameObject LoginFail;
    [SerializeField] private GameObject SginUpFail;

    private void Awake()
    {
        var bro = Backend.Initialize();
    }

    private void Start()
    {
        NickNameSetWin.SetActive(false);
    }

    public void LogginAcppt()
    {
        bool login = BackEndLogin.Instance.Login(IDInputField.text.ToString(), PinInputField.text.ToString());
        if (login)
        {
            IDInputField.text = string.Empty;
            PinInputField.text = string.Empty;
            OnSetNickObj();
        }
        else
        {
            IDInputField.text = string.Empty;
            PinInputField.text = string.Empty;
            LoginFail.SetActive(true);
        }
    }
    
    public void SginUp()
    {
        bool sginup = BackEndLogin.Instance.SignUp(SginUpIDInputField.text.ToString(),SginUpPinInputInputField.text.ToString());
        if (sginup)
        {
            SginUpIDInputField.text = string.Empty;
            SginUpPinInputInputField.text = string.Empty;
        }
        else
        {
            SginUpFail.SetActive(true);
        }
    }

    public void SetNickName()
    {
        string setnickname = NickNameInputInputField.text.ToString();
        bool bro = BackEndLogin.Instance.NickNameChage(setnickname);
        if (bro)
        {
            NickNameSetWin.SetActive(false);
            SceneManager.LoadScene("CTJMachine");
        }
        else
        {
            ChageNameFail.SetActive(true);
        }
    }

    public void OnSetNickObj()
    {
        string nickname = Backend.UserNickName;

        if (string.IsNullOrEmpty(nickname))
        {
            NickNameSetWin.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("CTJMachine");
        }
    }


}
