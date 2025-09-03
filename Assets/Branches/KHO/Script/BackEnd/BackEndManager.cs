using BackEnd;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

public class BackEndManager : MonoBehaviour
{
    public string id = string.Empty;
    public string pin = string.Empty;

    private void Awake()
    {
        var bro = Backend.Initialize();
        TestIntser();
    }

    private void TestIntser()
    {
        //BackEndLogin.Instance.SignUp(id, pin);
        BackEndLogin.Instance.Login(id, pin);
        //BackEndLogin.Instance.NickNameChage("±èÇÑ¿ï");
        MoneyGameData.Intance.GetData();
        // MoneyGameData.Intance.GameDateInsert();
        //MoneyGameData.Intance.GetData();
        //MoneyGameData.Intance.UpdateDate();
        //BackEndRank.Instance.RankInsert(1000);
        //BackEndRank.Instance.RankGet();
    }
}
