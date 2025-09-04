using BackEnd;
using UnityEngine;

public class BackEndManager : MonoBehaviour
{
    public string id = string.Empty;
    public string pin = string.Empty;
    private long best;
    private MoneyManager moneyManager;
    private void Awake()
    {
        var bro = Backend.Initialize();
        moneyManager = GetComponentInChildren<MoneyManager>();
        TestIntser();
    }

    private void TestIntser()
    {
        //BackEndLogin.Instance.SignUp(id, pin);
        BackEndLogin.Instance.Login(id, pin);
        MoneyGameData.Intance.GetData(ref best);
        moneyManager.Setbeest(best);
        //BackEndLogin.Instance.NickNameChage("±èÇÑ¿ï");
        // MoneyGameData.Intance.GameDateInsert();
        //MoneyGameData.Intance.GetData();
        //MoneyGameData.Intance.UpdateDate();
        //BackEndRank.Instance.RankInsert(1000);
        //BackEndRank.Instance.RankGet();
    }
}
