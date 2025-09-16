using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

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

    public int _compensation = 3;
    public int _rounds = 1;
    private bool a;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private TextMeshProUGUI _currentBankText;
    [SerializeField] private TextMeshProUGUI _interestText;
    [SerializeField] private GameObject Dead;
    private ShopPanel _shopPanel;

    private TargetAmountDown _amountDown;
    private bool _onActived;
    private AudioSource audio;
    [SerializeField] private AudioClip moneysound;
    public bool Oninterest { get; set; }
    private ShopPanel shopPanel;

    [Header("Fade In And Out")]
    [SerializeField] private FadeInAndOut _inAndOut;
    [SerializeField] private TextMeshProUGUI _inAndOutText;
    public int _inAndOutint = 1;

    [SerializeField] private TextMeshProUGUI _stageText;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        _moneyManager = MoneyManager.Instance;
        rate = FindAnyObjectByType<InterestRate>();
        _amountDown = FindAnyObjectByType<TargetAmountDown>();
        _shopPanel = FindAnyObjectByType<ShopPanel>();
        if (_currentBankText != null)
            _currentBankBaseColor = _currentBankText.color;
        else
            Debug.LogWarning("_currentBankText is not assigned in Inspector.");
    }
    private void Start()
    {
        _inAndOut.gameObject.SetActive(false);
        _conditionText.text = $"데드라인 : {_condition.ToString("N0")}";//데드라인 조건 표시
        _currentBankText.text = $"입금한 금액{_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시
        _stageText.text = $"Stage {_inAndOutint} - {_rounds}";

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
        if (_shopPanel.onActive == true)
        {
            long abc = _bankBook * aa / 100;
            _interestText.text = "이자:" + abc.ToString();

        }
        if (_shopPanel.onActive == true)
        {
            Oninterest = false;
        }
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
                _conditionText.text = "데드라인 : " + _condition;
            }
            else if (!_amountDown.TargetDown() && _onActived)
            {
                _condition /= 19;
                _condition *= 20;
                _onActived = false;
                _conditionText.text = "데드라인 : " + _condition;
            }
        }
    } //56번 줄 부터 내가 추가
    public void InMoney()//버튼에 이벤트
    {
        if (oninterRest == false)
        {
            //현재 소지금에 x%만큼 차감
            aaa = _condition * 1 / 10;
            if (_moneyManager.Money >= aaa)
            {
                _moneyManager.Money -= aaa;
                //차감한 금액만큼 증가
                _bankBook += aaa;
                //Ui갱신 
                _creditsText.text = $"보유 금액 : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신\
                logUI.AddLog($"-{aaa.ToString("N0")} 입금 : {_moneyManager.Money.ToString("N0")}", Color.red);
                _currentBankText.text = $"입금한 금액: {_bankBook.ToString("N0")}";//현재까지 입금된 금액 표시
            }
            else logUI.AddLog($"실패 : 돈이 부족합니다 필요금액 {aaa}", Color.red);
            audio.PlayOneShot(moneysound);
        }
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
            _sloltMahcin.HaveSpin += _compensation * 2;
            _sloltMahcin.ButtonTrue();
            _condition *= 2;
            _compensation = 3;
            _rounds = 1;
            _conditionText.text = $"데드라인 : {_condition.ToString("N0")}";
            _sloltMahcin.UpdateMagnificationUI();
            StartCoroutine(FadeSequence());
            _inAndOutint++;
            StartCoroutine(_sloltMahcin.PlayHorizontalMatchEffects());
            _stageText.text = $"Stage {_inAndOutint} - {_rounds}";
            _buttonDisabledOnce = false;
        }
        else if (_bankBook < _condition && _compensation <= 0)
        {
            Dead.SetActive(true);
            a = true;
        }
    }

    public void MoneyP()
    {
        if (Oninterest == false)
        {
            long abc = _bankBook * aa / 100;
            bool async = false;
            if (!async)
            {
                //남은 라운드 수 차감
                //현재 입금된 금액의 x%만큼 돈 지급
                _moneyManager.Money += abc;
                _creditsText.text = $"보유 금액 : {_moneyManager.Money.ToString("N0")}";//현재 소유한 금액 갱신
                _stageText.text = $"Stage {_inAndOutint} - {_rounds}";
                logUI.AddLog($"+{abc.ToString("N0")} 이자 : {_moneyManager.Money.ToString("N0")}", Color.green);

            }
            audio.PlayOneShot(moneysound);
            Oninterest = true;
            async = true;

        }
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
    private IEnumerator FadeSequence()
    {
        _inAndOut.gameObject.SetActive(true);

        yield return StartCoroutine(_inAndOut.StartFadeIn());
        _inAndOutText.text = $"{_inAndOutint} - {_rounds} 스테이지";
        yield return new WaitForSeconds(0.6f);
        yield return StartCoroutine(_inAndOut.StartFadeStart());

        _inAndOut.gameObject.SetActive(false);
    }
}
