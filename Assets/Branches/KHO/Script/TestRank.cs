using UnityEngine;
using BackEnd;

public class TestRank : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private string pw;

    private void Awake()
    {
        Backend.Initialize();
        TestLogin();
    }

    public void TestLogin()
    {
        BackEndLogin.Instance.Login(id, pw);
    }
}
