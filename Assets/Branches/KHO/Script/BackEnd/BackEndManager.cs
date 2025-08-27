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
        BackEndLogin.Instance.Login("user1", "1234");
        // MoneyGameData.Intance.GameDateInsert();
        //MoneyGameData.Intance.GetData();
        //MoneyGameData.Intance.UpdateDate();
        //BackEndRank.Instance.RankInsert(1000);
        //BackEndRank.Instance.RankGet();
        //BackEndLogin.Instance.NickNameChage("¹ÚÃ¶¹Î");.
    }
}
