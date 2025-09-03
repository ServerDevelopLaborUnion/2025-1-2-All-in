using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private SloltMachine _sloltMahcin;
    public MoneyLogUI logUI;


    [Header("현재까지 입금된 금액")]
    [SerializeField] private long _bankBook;
    private long aaa;
    [Header("이자율 ex)10 ->10%")]
    [SerializeField] private long aa;
    [Header("데드라인 조건")]
    [SerializeField] private long _condition;

    public int _rounds = 3;
    private bool a;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private TextMeshProUGUI _currentBankText;
    private void Start()
    {
        _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";//데드라인 조건 표시
        _currentBankText.text = $"Current deposit amount : {_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시

    }

    private void Update()
    {
        if (!a)
            CheckMoney();

        MoneyP();
    }
    public void InMoney()//버튼에 이벤트
    {
        //현재 소지금에 x%만큼 차감
        aaa = _condition / 10;
        _moneyManager.Money -= aaa;
        //차감한 금액만큼 증가
        _bankBook += aaa;
        //Ui갱신 
        _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신\
        logUI.AddLog($"-{aaa.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}", Color.red);
        _currentBankText.text = $"Current deposit amount : {_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시
    }

    public void CheckMoney()
    {
        //증가된 금액이 조건에 맞는지 확인

        if (_bankBook >= _condition)
        {
            //맞다면 다음조건 제시
            _condition = _condition * 2;
            _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";//데드라인 조건 표시
            _sloltMahcin.HaveSpin += 10;
            _sloltMahcin.UpdateMagnificationUI();
        }
        else if (_bankBook < _condition && _rounds == 0)
        {
            Debug.Log("게임오버");
            a = true;
        }
    }

    private void MoneyP()
    {
        long abc =_bankBook * aa / 100;
        bool async = false;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!async)
            {
                //남은 라운드 수 차감
                _rounds--;
                //현재 입금된 금액의 x%만큼 돈 지급
                _moneyManager.Money +=abc;
                _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신
                logUI.AddLog($"+{abc.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}",Color.green);


                //다음 라운드 실행 => 음.. 어떻게 만들지?
            }
        }
        async = true;

    }
}
