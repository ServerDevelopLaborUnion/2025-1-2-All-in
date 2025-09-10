using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeadLine : MonoBehaviour
{
    private MoneyManager _moneyManager;
    [SerializeField] private SloltMachine _sloltMahcin;
    public MoneyLogUI logUI;
    private InterestRate rate;
    private bool oninterRest = false;
    private Coroutine _blinkCoroutine;
    private Color _currentBankBaseColor;
    [SerializeField] private Color _alertColor = Color.red;
    [SerializeField] private float _blinkInterval = 0.5f;

    [SerializeField] private UnityEngine.UI.Button targetButton;
    private bool _buttonCooldown;
    private bool _buttonDisabledOnce;

    //이자율 ex)10 ->10%
    [field: SerializeField] public long aa { get; set; } = 2;

    [Header("현재까지 입금된 금액")]
    [SerializeField] private long _bankBook;
    private long aaa;

    [field: SerializeField] public long _condition { get; set; } = 100000;

    public int _rounds = 3;
    private bool a;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private TextMeshProUGUI _currentBankText;

    private TargetAmountDown _amountDown;
    private bool _onActived;
    private void Awake()
    {
        _moneyManager = MoneyManager.Instance;
        rate = FindAnyObjectByType<InterestRate>();
        _amountDown = FindAnyObjectByType<TargetAmountDown>();
        _moneyManager = MoneyManager.Instance;
        rate = FindAnyObjectByType<InterestRate>();
        _amountDown = FindAnyObjectByType<TargetAmountDown>();
        if (_currentBankText != null) _currentBankBaseColor = _currentBankText.color;
        else Debug.LogWarning("_currentBankText is not assigned in Inspector.");
    }
    private void Start()
    {
        _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";//데드라인 조건 표시
        _currentBankText.text = $"{_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시

    }

    private void Update()
    {
        if (!a)
            CheckMoney();

        if (rate != null && oninterRest == false)
        {
            aa += rate.Interest();
            oninterRest = true;
        }
        targetAmount();
    }
    private void targetAmount()
    {
        if (_amountDown != null)
        {
            if (_amountDown.TargetDown() && !_onActived)
            {
                _condition /= 20;
                _condition *= 19;
                _onActived = true;
                _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";
            }
            else if (!_amountDown.TargetDown() && _onActived)
            {
                _condition /= 19;
                _condition *= 20;
                _onActived = false;
                _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";
            }
        }
    } //56번 줄 부터 내가 추가
    public void InMoney()//버튼에 이벤트
    {
        //현재 소지금에 x%만큼 차감
        aaa = _condition * 1 / 10;
        if (_moneyManager.Money >= aaa)
        {
            _moneyManager.Money -= aaa;
            //차감한 금액만큼 증가
            _bankBook += aaa;
            //Ui갱신 
            _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신\
            logUI.AddLog($"-{aaa.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}", Color.red);
            _currentBankText.text = $"{_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시
        }
        else logUI.AddLog($"Fall : ",Color.red);
    }

    public void CheckMoney()
    {
        if (_currentBankText == null) return;

        long threshold = _condition * 90 / 100; // 조건의 90%

        // 90% 도달 시 한 번만 버튼 비활성화
        if (_bankBook >= threshold && !_buttonCooldown && !_buttonDisabledOnce)
        {
            StartCoroutine(DisableButtonBriefly());
        }

        // 90% 이상이면 깜빡임
        if (_bankBook >= threshold)
        {
            if (_blinkCoroutine == null)
                _blinkCoroutine = StartCoroutine(BlinkUntilConditionChanged());
        }
        else
        {
            StopBlink();
        }

        // 조건 달성 시
        if (_bankBook >= _condition)
        {
            StopBlink();
            _sloltMahcin.HaveSpin += _rounds * 2;
            _sloltMahcin.ButtonTrue();
            _condition *= 2;
            _rounds = 3;
            _conditionText.text = $"DeadLine : {_condition.ToString("N0")}";
            _sloltMahcin.UpdateMagnificationUI();

            // 조건 달성 후 플래그 리셋 (다음 조건에서 다시 작동 가능)
            _buttonDisabledOnce = false;
        }
        else if (_bankBook < _condition && _rounds == 0)
        {
            Debug.Log("게임오버");
            a = true;
        }
    }

    public void MoneyP()
    {
        long abc = _bankBook * aa / 100;
        bool async = false;
        if (!async)
        {
            //남은 라운드 수 차감
            _rounds--;
            Debug.Log(_rounds);
            //현재 입금된 금액의 x%만큼 돈 지급
            _moneyManager.Money += abc;
            _creditsText.text = $"Credits : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신
            logUI.AddLog($"+{abc.ToString("N0")} balance : {_moneyManager.Money.ToString("N0")}", Color.green);
        }
        async = true;
    }

    private IEnumerator BlinkUntilConditionChanged()
    {
        while (_bankBook >= _condition * 90 / 100)
        {
            _currentBankText.color = _alertColor;
            yield return new WaitForSeconds(_blinkInterval);
            _currentBankText.color = _currentBankBaseColor;
            yield return new WaitForSeconds(_blinkInterval);
        }
        StopBlink();
    }

    private IEnumerator DisableButtonBriefly()
    {
        _buttonCooldown = true;

        if (targetButton != null)
        {
            targetButton.interactable = false;
            yield return new WaitForSeconds(1f);
            targetButton.interactable = true;
        }

        _buttonCooldown = false;
        _buttonDisabledOnce = true; // 이미 한 번 처리했음 표시
    }

    private void StopBlink()
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
        }
        _currentBankText.color = _currentBankBaseColor;
    }
}
