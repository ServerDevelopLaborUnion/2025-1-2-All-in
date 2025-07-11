using UnityEngine;

public class SelectChecker : MonoBehaviour
{
    [SerializeField] GameObject Test;

    private void Awake()
    {
        Test.SetActive(false);
    }

    public void GetActive()
    {
        Test.SetActive(true);
    }

    public void ExitActive()
    {
        Test.SetActive(false);
    }
}
