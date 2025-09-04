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


    public void SignUp(string id, string pin)
    {
        var bro = Backend.BMember.CustomSignUp(id.Trim(), pin.Trim());
    }


    public void Login(string id, string pin)
    {
        var bro = Backend.BMember.CustomLogin(id.Trim(), pin.Trim());

        if (bro.IsSuccess())
        {
            // JSON에서 nickname 값 가져오기
            string nickname = bro.GetReturnValuetoJSON()["nickname"].ToString();

            if (string.IsNullOrEmpty(nickname))
            {
                //나중에 UI 쪽에서 해결 후 기제
            }
        }
    }

    public void NickNameChage(string nickname)
    {
        var bro = Backend.BMember.CheckNicknameDuplication(nickname);
        if (bro.IsSuccess())
        {
            var setNicknamebro = Backend.BMember.UpdateNickname(nickname);
        }
        else
        {
            Debug.Log("중복된 닉네임입니다");
        }
    }
}
