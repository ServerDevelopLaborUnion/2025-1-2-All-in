using TMPro;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private MoneyManager _moneyManager;
    private long _bankBook;
    private long _interest;
    private long _condition;
    private int _rounds;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _bankBookText;
    public void InMoney()//버튼에 이벤트
    {
        //현재 소지금에 x%만큼 차감
        _interest = _moneyManager.Money / 10;
        //차감한 금액만큼 증가
        _bankBook += _interest;
        //Ui갱신 
        _creditsText.text = "Credits :" + _moneyManager.Money;//현재 소유한 금액 갱신
        _bankBookText.text = "DeadLine :" + _bankBook;//현재 소유한 금액 갱신
        CheckMoney();
    }

    private void CheckMoney()
    {
        //증가된 금액이 조건에 맞는지 확인
        if (_bankBook >= _condition)
        {
            //맞다면 다음조건 제시
            _condition = _condition * 2;
        }
        else if (_rounds > 0)//아니면 현재 남은 라운드x가 x > 0인지 확인
        {
            //맞다면 다움 라운드 실행
            MoneyP();
        }
        else
        {
            //아니면 게임 오버
            //대충 팝업창 띄우는게 좋을 듯
        }
    }

    private void MoneyP()
    {
        bool async = false;
        if (!async)
        {
            //남은 라운드 수 차감
            _rounds--;
            //현재 입금된 금액의 x%만큼 돈 지급
            _moneyManager.Money += _bankBook / 10;
            _creditsText.text = "Credits :" + _moneyManager.Money;//현재 소유한 금액 갱신

            //다음 라운드 실행 => 음.. 어떻게 만들지?
        }
        async = true;

    }
}
