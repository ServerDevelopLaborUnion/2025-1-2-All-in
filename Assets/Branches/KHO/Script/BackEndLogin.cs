using UnityEngine;
using BackEnd;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;

public class BackEndLogin
{
    private static BackEndLogin _instance;

    public static BackEndLogin Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackEndLogin();
            }

            return _instance;
        }
    }


    public void SignUp(string id, string pin)
    {
        var bro = Backend.BMember.CustomSignUp(id, pin);
    }


    public void Login(string id, string pin)
    {
        var bro = Backend.BMember.CustomLogin(id, pin);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인 성공");
        }

    }

    public void NickNameChage(string nickname)
    {
        var bro = Backend.BMember.UpdateNickname(nickname);
    }
}
