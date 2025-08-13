using UnityEngine;

public class BackgroundRunManager : MonoBehaviour
{
    void Awake()
    {
        // 게임이 백그라운드 상태에서도 계속 실행되도록 설정
        Application.runInBackground = true;

        // (선택) 화면 꺼짐 방지 (특히 모바일에서)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
