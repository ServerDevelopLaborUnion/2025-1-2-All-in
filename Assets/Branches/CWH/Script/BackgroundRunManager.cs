using UnityEngine;

public class BackgroundRunManager : MonoBehaviour
{
    void Awake()
    {
        // ������ ��׶��� ���¿����� ��� ����ǵ��� ����
        Application.runInBackground = true;

        // (����) ȭ�� ���� ���� (Ư�� ����Ͽ���)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
