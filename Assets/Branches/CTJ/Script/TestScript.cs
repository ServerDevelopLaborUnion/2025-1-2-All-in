using UnityEngine;

public class TestScript : MonoBehaviour
{
    float times = 0;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        times += Time.deltaTime;
        Debug.Log(times);
    }
}
