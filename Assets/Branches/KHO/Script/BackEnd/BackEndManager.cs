using UnityEngine;
using BackEnd;

public class BackEndManager : MonoBehaviour
{
    public string id = string.Empty;
    public string pin = string.Empty;

    void Start()
    {
        var bro = Backend.Initialize();
        TestIntser();
    }

    private void TestIntser()
    {
        BackEndLogin.Instance.Login(id, pin);
        // MoneyGameData.Intance.GameDateInsert();
        //MoneyGameData.Intance.GetData();
        //MoneyGameData.Intance.UpdateDate();
        //BackEndRank.Instance.RankInsert(1000);
        //BackEndRank.Instance.RankGet();
    }
}
