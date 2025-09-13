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
        Backend.Initialize();
        moneyManager = GetComponentInChildren<MoneyManager>();
        TestIntser();
    }

    private void TestIntser()
    {
        BackEndLogin.Instance.Login(id,pin);
        MoneyGameData.Intance.GetData(ref best);
        moneyManager.Setbeest(best);
    }
}
