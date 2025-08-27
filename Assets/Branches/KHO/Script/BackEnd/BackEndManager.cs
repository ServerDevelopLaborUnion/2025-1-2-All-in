using UnityEngine;
using BackEnd;

public class BackEndManager : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize();
        TestIntser();
    }

    private void TestIntser()
    {
        BackEndLogin.Instance.SignUp("ur2","12345");
        BackEndLogin.Instance.Login("ur2", "12345");
        BackEndLogin.Instance.NickNameChage("¡∂≈¬¡ÿ");
        // MoneyGameData.Intance.GameDateInsert();
        //MoneyGameData.Intance.GetData();
        //MoneyGameData.Intance.UpdateDate();
        //BackEndRank.Instance.RankInsert(1000);
        //BackEndRank.Instance.RankGet();
    }
}
