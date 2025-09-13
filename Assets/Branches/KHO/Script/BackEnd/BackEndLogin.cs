using UnityEngine;
using BackEnd;

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


    public bool SignUp(string id, string pin)
    {
        var bro = Backend.BMember.CustomSignUp(id.Trim(), pin.Trim());

        if (bro.IsSuccess())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool Login(string id, string pin)
    {
        var bro = Backend.BMember.CustomLogin(id.Trim(), pin.Trim());
        if (bro.IsSuccess())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NickNameChage(string nickname)
    {
        var bro = Backend.BMember.CheckNicknameDuplication(nickname);
        if (bro.IsSuccess())
        {
            var setNicknamebro = Backend.BMember.UpdateNickname(nickname);
            return true;
        }
        else
        {
            return false;
        }
    }
}
