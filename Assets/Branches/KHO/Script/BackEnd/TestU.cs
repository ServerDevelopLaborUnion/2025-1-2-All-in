using UnityEngine;

public class TestU : MonoBehaviour
{
    public string id = string.Empty;
    public string pin = string.Empty;

    private void Start()
    {
        BackEndLogin.Instance.SignUp(id, pin);
    }
}
