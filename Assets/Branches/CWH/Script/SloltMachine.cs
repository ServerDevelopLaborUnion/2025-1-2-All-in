using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class SloltMachine : MonoBehaviour
{
    //오토 스핀용코드는 임시로 사용중 -원희
    #region 오토 스핀용 코드 
    public long GetCredits() => credits;
    public long GetMinimumBet() => _minBet;
    public bool IsSpinning() => isStartSpin;


    public void SetBetAmount(long bet)
    {
        inputBetAmount.text = bet.ToString();
    }

    public bool IsJackpotHit(long betAmount)
    {
        return CheckJackpot(betAmount);
    }
    #endregion

    private long lastBetAmount;
    private bool fallChecked;


    [Header("돈")]
    [SerializeField] private long credits = 100;
    public long Credits
    {
        get { return credits; }
        set
        {
            if (value < 0) credits = 0;
            credits = value;
        }
    }

    [SerializeField] private TMP_InputField inputBetAmount;
    [SerializeField] private Image imageBetAmount;
    [SerializeField] private TextMeshProUGUI textCredits;
    [SerializeField] private TextMeshProUGUI _minBetText;
    private long _minBet;

    [Header("릴 텍스트//게임에 보이는 것")]
    [SerializeField] private TextMeshProUGUI[] reelTextsFlat = new TextMeshProUGUI[15];

    [Header("릴 이미지//게임에 보이는 것")]
    [SerializeField] private Image[] reelImagesFlat = new Image[15];


    [Header("카메라")]
    [SerializeField] private Transform cameraTransform;


    [Header("파티클")]
    [SerializeField] private ParticleSystem horizontalMatchParticle;

    [Header("배팅 배율")]
    [SerializeField] private int magnification;
    [SerializeField] private TextMeshProUGUI _magnificationText;

    [Header("남은 스핀 수 (보류)")]
    [SerializeField] private TMPro.TextMeshProUGUI _numberOfSpinsreMaining;
    [SerializeField] private int _haveSpin;
    public int HaveSpin
    {
        get { return _haveSpin; }
        set 
        {
            if (value < 0) _haveSpin = 0;
            _haveSpin = value;
            
        }
    }
    [SerializeField] private int _spinCost = 1;
    public int SpinCost
    {
        get { return _spinCost; }
        set
        {
            if(value < 0) _spinCost = 0;
            _spinCost = value;
        }
    }

    #region 잭팟확률 관련
    [Header("잭팟")]
    [SerializeField] private float jackpotChance = 0.00001f;
    private const float jackpotChanceMax = 0.5f;
    private const float jackpotChanceIncrement = 0.0000001f;
    private const float jackpotChanceInitial = 0.000000005f;

    #endregion
    [Header("세로")]
    [SerializeField] private float _verticalChance;
    [Header("가로")]
    [SerializeField] private float _horizontalChance;

    //텍스트, 버튼
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textChance;
    [SerializeField] private Button pullButton;
    [SerializeField] private Button allInButton;
    [SerializeField] private Button pButton;
    [SerializeField] private Button mButton;



    //릴 내부적으로 돌아가는 거
    private int[,] reelResults = new int[3, 5];
    private Image[,] reelImages = new Image[3, 5];
    private TextMeshProUGUI[,] reelTexts = new TextMeshProUGUI[3, 5];

    //스핀 돌아가는시간,경과 시간 , 스핀 가능 유무
    private float spinDuration = 0.2f;
    private float elapsedTime = 0f;
    private bool isStartSpin = false;

    //각 릴이 돌라가는 것
    private Coroutine[] reelSpinCoroutines = new Coroutine[5];
    //각 릴의 멈춤
    private bool[] isReelSpinned = new bool[5];

    //색깔들
    Color32 customMatch = new Color32(255, 239, 184, 255);
    Color32 customJackPot = new Color32(207, 255, 182, 255);

    private void Awake()
    {
        _minBet = credits / 50;
        credits = Math.Clamp(credits, 0, long.MaxValue / 2);
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                reelImages[row, col] = reelImagesFlat[row * 5 + col];
                reelTexts[row, col] = reelTextsFlat[row * 5 + col];
            }
        }
        UpdateMagnificationUI();
        textCredits.text = $"Credits : {credits.ToString("N0")}";
        _minBetText.text = $"Minimum bet \n {_minBet.ToString("N0")}";
        textChance.text = $"Probability Table\n Vertical : 15% \n Horizontal : 5% \n Jackpot : {jackpotChance * 100:F4}%";
        _magnificationText.text = $"Current Magnification\n" +
                                  $" Vertical : {magnification * 2}x" +
                                  $"\n Horizontal : {magnification * 4}x" +
                                  $"\n Jackpot : {magnification * 1000}x" +
                                  $"\n Fall : -{magnification * 3}x";
        _numberOfSpinsreMaining.text = $"Number of spins remaining \n {_haveSpin} \n Spin Cost {_spinCost}";
    }

    private void Update()
    {
        if (!isStartSpin) return;

        elapsedTime += Time.deltaTime;

        for (int col = 0; col < 5; col++)
        {
            if (!isReelSpinned[col] && elapsedTime >= spinDuration)
            {
                ApplyVerticalMatch(col);
                isReelSpinned[col] = true;
                elapsedTime = 0f;
                break;
            }
        }

        if (AllReelsSpinned())
        {
            isStartSpin = false;
            ResetReelSpins();
            UpdateReelDisplay();
            CheckBet();
        }
    }

    private void ApplyVerticalMatch(int col)
    {
        int baseSpin = UnityEngine.Random.Range(1, 8);
        bool forceVerticalMatch = UnityEngine.Random.value < _verticalChance;
        Debug.Log("세로줄");

        for (int row = 0; row < 3; row++)
        {
            reelResults[row, col] = forceVerticalMatch ? baseSpin : UnityEngine.Random.Range(1, 8);
        }
    }

    private void ApplyHorizontalMatch()
    {
        Debug.Log("가로줄");

        int matchRowCount = UnityEngine.Random.Range(1, 3); // 1~2줄 매칭
        List<int> rows = new List<int> { 0, 1, 2 };
        for (int i = 0; i < rows.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, rows.Count);
            (rows[i], rows[j]) = (rows[j], rows[i]);
        }
        for (int i = 0; i < matchRowCount; i++)
        {
            int row = rows[i];
            int value = UnityEngine.Random.Range(1, 8);
            for (int col = 0; col < 5; col++)
            {
                reelResults[row, col] = value;
            }
        }
    }


    private void ApplyJackpot()
    {
        int jackpotSymbol = UnityEngine.Random.Range(1, 8);
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 5; col++)
                reelResults[row, col] = jackpotSymbol;
    }

    private void UpdateReelDisplay()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                reelTexts[row, col].text = reelResults[row, col].ToString("D1");
            }
        }
    }

    private void ResetReels()
    {
        foreach (var img in reelImagesFlat)
            img.color = Color.white;

        foreach (var txt in reelTextsFlat)
            txt.color = Color.black;

        OnMessage(Color.white, string.Empty);
    }

    public void OnMoney()
    {
        credits = 100000;
        textCredits.text = $"Credits : {credits.ToString("N0")}";
    }

    public void OnClickpull()
    {
        ResetReels();


        horizontalMatchParticle.Stop();
        if (!long.TryParse(inputBetAmount.text.Trim(), out long bet) || bet < _minBet)
        {
            OnMessage(Color.red, "Invalid bet amount");
            return;
        }

        if (credits < bet)
        {
            OnMessage(Color.red, "You don't have enough money");
            return;
        }

        credits -= bet;
        lastBetAmount = bet;   // 이번 스핀의 베팅 금액 저장
        fallChecked = false;   // Fall 체크 초기화

        textCredits.text = $"Credits : {credits.ToString("N0")}";
        //StartSpin();
        EnoughSpin();
    }

    public void EnoughSpin()
    {

        if (_haveSpin <= 0 || _haveSpin < _spinCost)
        {
            pullButton.interactable = false;
            allInButton.interactable = false;
            OnMessage(Color.white, "You don't have enough Spin");
            return;
        }
        else
        {
            StartSpin();
            _haveSpin -= _spinCost;
            UpdateMagnificationUI();
        }

    }

    public void OnSpinP()
    {
        if (credits < 10000)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }

        credits -= 10000;
        _haveSpin += 1;
        pullButton.interactable = true;
        allInButton.interactable = true;
        textCredits.text = $"Credits : {credits.ToString("N0")}";
        UpdateMagnificationUI();
    }

    public void OnClickP()
    {
        if (credits < 10)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }
        credits -= magnification * magnification;
        _spinCost = Mathf.Clamp(_spinCost += 2, 1, 20);
        magnification = Mathf.Clamp(magnification + 1, 1, 20);

        UpdateMagnificationUI();
    }

    public void OnClickM()
    {
        if (credits < 10)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }
        credits -= magnification * 2;
        _spinCost = Mathf.Clamp(_spinCost -= 2, 1, 20);
        magnification = Mathf.Clamp(magnification - 1, 1, 20);

        UpdateMagnificationUI();
    }

    private void UpdateMagnificationUI()
    {
        // 버튼 상태 갱신
        mButton.interactable = magnification > 1;
        pButton.interactable = magnification < 20;

        if (magnification <= 2)
            _magnificationText.text = $"Current Magnification\n" +
                                      $" Vertical : {magnification * 2}x" +
                                      $"\n Horizontal : {magnification * 4}x" +
                                      $"\n Jackpot : {magnification * 1000}x" +
                                      $"\n Fall : -{magnification * 3}x";

        else _magnificationText.text = $"Current Magnification\n" +
                              $" Vertical : {magnification * 2}x" +
                              $"\n Horizontal : {magnification * 4}x" +
                              $"\n Jackpot : {magnification * 1000}x" +
                              $"\n Fall : -{(magnification + 5) * 3}x";

        textCredits.text = $"Credits : {credits:N0}";
        _numberOfSpinsreMaining.text = $"Number of spins remaining \n {_haveSpin} \n Spin Cost {_spinCost}";
    }

    private void StartSpin()
    {
        isStartSpin = true;
        pullButton.interactable = false;
        allInButton.interactable = false;
        elapsedTime = 0;
        ResetReelSpins();

        // 기본 랜덤 결과 생성
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 5; col++)
                reelResults[row, col] = UnityEngine.Random.Range(1, 8);

        // 세로줄 매치 확률 적용
        //for (int col = 0; col < 5; col++)
        //{
        //    if (UnityEngine.Random.value < 0.1f)
        //    {
        //        int val = UnityEngine.Random.Range(1, 8);
        //        for (int row = 0; row < 3; row++)
        //            reelResults[row, col] = val;
        //    }
        //}

        float rand = UnityEngine.Random.value;

        if (rand < jackpotChance)
        {
            ApplyJackpot();

        }
        else if (rand < _horizontalChance)
        {
            ApplyHorizontalMatch();
            jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
        }
        else
        {
            jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
        }

        // 릴 스핀 시작
        for (int col = 0; col < 5; col++)
        {
            if (reelSpinCoroutines[col] != null)
                StopCoroutine(reelSpinCoroutines[col]);

            reelSpinCoroutines[col] = StartCoroutine(SpinReelLoop(col));
        }

        StartCoroutine(StopReelsOneByOne());
    }
    public void OnClickMinimumbet()
    {
        if (credits <= 0)
        {
            OnMessage(Color.red, "You have no credits");
            return;
        }

        inputBetAmount.text = _minBet.ToString();
        OnClickpull();
    }

    private void CheckBet()
    {
        bool hasMatch = false;

        foreach (var img in reelImagesFlat)
            img.color = Color.white;

        if (CheckJackpot(lastBetAmount))
            return;

        bool vertical = CheckVertical(lastBetAmount);
        bool horizontal = CheckHorizontal(lastBetAmount);
        bool jackpot = CheckJackpot(lastBetAmount);
        hasMatch = vertical || horizontal;

        _minBet = credits / 50;
        if (_minBet == 0)
            _minBet += 1;

        if (credits >= long.MaxValue / 2)
            CreditMaxOver();

        if (!hasMatch)
        {
            CheckFall();
        }

        _minBetText.text = $"Minimum bet \n {_minBet.ToString("N0")}";
        textCredits.text = $"Credits : {credits.ToString("N0")}";
        textChance.text = $"Probability Table\n Vertical : 15% \n Horizontal : 5% \n Jackpot : {jackpotChance * 100:F4}%";
        textResult.text = hasMatch ? "YOU WIN!!!" : "YOU LOSE!!!!";

        if (horizontal)
        {
            StartCoroutine(PlayHorizontalMatchEffects());
        }
        else if (jackpot)
        {
            StartCoroutine(PlayHorizontalMatchEffects());
        }
    }

    #region 코루틴
    private IEnumerator BlinkText(TextMeshProUGUI text, float duration, float interval)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
        text.enabled = true;
    }


    private IEnumerator SpinReelLoop(int col)
    {
        while (!isReelSpinned[col])
        {
            for (int row = 0; row < 3; row++)
            {
                int randVal = UnityEngine.Random.Range(1, 8);
                reelTexts[row, col].text = randVal.ToString();
            }
            yield return new WaitForSeconds(0.05f);
        }

        // 최종 결과 표시
        for (int row = 0; row < 3; row++)
        {
            reelTexts[row, col].text = reelResults[row, col].ToString("D1");
        }
    }
    private IEnumerator StopReelsOneByOne()
    {
        for (int col = 0; col < 5; col++)
        {
            yield return new WaitForSeconds(0.2f); // 릴 간 멈추는 간격
            isReelSpinned[col] = true;             // 이 릴 멈춤
        }

        yield return new WaitForSeconds(0.2f);

        isStartSpin = false;

        CheckBet();
        pullButton.interactable = true;
        allInButton.interactable = true;
    }

    private IEnumerator PlayHorizontalMatchEffects()
    {
        // 파티클 재생 (예: particleSystem.Play();)
        horizontalMatchParticle.Play();

        // 화면 흔들기 효과 실행
        yield return StartCoroutine(ScreenShakeCoroutine(0.5f, 0.01f));

    }

    private IEnumerator ScreenShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
    #endregion
    private void OnMessage(Color color, string msg)
    {
        imageBetAmount.color = color;
        textResult.text = msg;
    }
    private bool CheckVertical(long bet)
    {
        bool matched = false;

        for (int col = 0; col < 5; col++)
        {
            int a = reelResults[0, col];
            int b = reelResults[1, col];
            int c = reelResults[2, col];

            if (a == b && b == c)
            {
                matched = true;
                AddCredits(bet * (magnification * 2));

                for (int row = 0; row < 3; row++)
                {
                    reelImages[row, col].color = customMatch;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.2f, 0.15f));
                }
            }
        }

        return matched;
    }

    private bool CheckHorizontal(long bet)
    {
        bool matched = false;

        for (int row = 0; row < 3; row++)
        {
            int a = reelResults[row, 0];
            int b = reelResults[row, 1];
            int c = reelResults[row, 2];
            int d = reelResults[row, 3];
            int e = reelResults[row, 4];

            if (a == b && b == c && c == d && d == e)
            {
                matched = true;
                AddCredits(bet * (magnification * 4));

                for (int col = 0; col < 5; col++)
                {
                    reelImages[row, col].color = customMatch;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.15f));
                }
            }
        }

        return matched;
    }

    private bool CheckJackpot(long betAmount)
    {
        int first = reelResults[0, 0];

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                if (reelResults[r, c] != first)
                    return false;


        jackpotChance = jackpotChanceInitial;
        // 잭팟 처리
        textResult.text = " JACKPOT!!! ";
        AddCredits(betAmount * (magnification * 1000));
        textCredits.text = $"Credits : {credits.ToString("N0")}";

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
            {
                reelImages[r, c].color = customJackPot;
                StartCoroutine(BlinkText(reelTexts[r, c], 0.5f, 0.15f));
            }
        return true;
    }
    private bool CheckFall()
    {
        if (fallChecked) return false; // 이미 체크했으면 중복 방지
        fallChecked = true;

        if (magnification <= 2)
            credits -= lastBetAmount * 3;
        else
            credits -= lastBetAmount * (magnification + 5) * 3;

        credits = Math.Clamp(credits, 0, long.MaxValue / 2);
        if (credits < 0)
        {
            CreditMinOver();
        }
        return true;
    }
    private void AddCredits(long amount)
    {
        try
        {
            checked
            {
                credits += amount;
            }
        }
        catch (OverflowException)
        {
            credits = long.MaxValue; // 상한으로 고정
        }

        credits = Math.Clamp(credits, 0, long.MaxValue);
    }


    private void CreditMinOver()
    {
        credits = 0;
    }

    private void CreditMaxOver()
    {
        credits = long.MaxValue / 2;
    }

    private void ResetReelSpins()
    {
        for (int i = 0; i < 5; i++)
            isReelSpinned[i] = false;
    }

    private bool AllReelsSpinned()
    {
        foreach (bool b in isReelSpinned)
            if (!b) return false;
        return true;
    }
}
