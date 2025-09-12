using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class SloltMachine : MonoBehaviour
{
    public enum SpinPattern
    {
        Jackpot, Horizontal, Vertical, Normal
    }
    //오토 스핀용코드는 임시로 사용중 -원희
    #region 오토 스핀용 코드 
    public long GetCredits() => credits.Money;
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
    public List<ItemOn> items = new List<ItemOn>();
    public long lastBetAmount;
    private bool fallChecked;

    [Header("돈")]
    private MoneyManager credits;
    [SerializeField] private long _startCredits;

    [SerializeField] private TMP_InputField inputBetAmount;
    [SerializeField] private Image imageBetAmount;
    [SerializeField] private TextMeshProUGUI textCredits;
    [SerializeField] private TextMeshProUGUI _minBetText;
    [SerializeField] private long _minBet;
    [SerializeField] private long _maxBet;

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
    [SerializeField] private TMPro.TextMeshProUGUI _remainSpins;
    [SerializeField] private TMPro.TextMeshProUGUI _SpinCosts;
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
            if (value < 0) _spinCost = 0;
            _spinCost = value;
        }
    }

    public int _spinCoststandard;

    #region 잭팟확률 관련
    [Header("잭팟")]
    public float jackpotChance = 0.00001f;
    private const float jackpotChanceMax = 0.5f;
    private const float jackpotChanceIncrement = 0.0000001f;
    private const float jackpotChanceInitial = 0.000000005f;

    #endregion
    [Header("세로")]
    [field: SerializeField] public float _verticalChance;
    public float VerticalChance { get; set; }
    [Header("가로")]
    [field: SerializeField] public float _horizontalChance;
    public float HorizonTalChance { get; set; }

    public MoneyLogUI logUI;

    //텍스트, 버튼
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textChance;
    public Button pullButton;
    public Button minBetButton;
    public Button maxBetButton;
    public Button pButton;
    public Button mButton;

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

    [Header("화면 흔들기용UI")]
    [SerializeField] private RectTransform[] uiCanvases;      // 흔들 UI Canvas 배열
    [SerializeField] private LayoutGroup[] layoutGroups;      // 각 Canvas LayoutGroup (없으면 null)

    private bool _this;
    bool _startSpinbug = true;

    private void Awake()
    {
        credits = MoneyManager.Instance;
    }

    private void Start()
    {
        if (cameraTransform == null)
            Debug.LogError("카메라 Transform이 할당되지 않았습니다!");
        credits.Money = _startCredits;
        credits.Money = Math.Clamp(credits.Money, 0, long.MaxValue / 2);
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                reelImages[row, col] = reelImagesFlat[row * 5 + col];
                reelTexts[row, col] = reelTextsFlat[row * 5 + col];
            }
        }
        EnoughSpin();
        UpdateMagnificationUI();
        textCredits.text = $"보유 금액 : {credits.Money.ToString("N0")}원";
        _minBetText.text = $"최소 베팅금 : {_minBet.ToString("N0")}원";
        textChance.text = $" 가로줄 : {_verticalChance * 100}% \n 세로줄 : {_horizontalChance * 100}% \n 잭팟 : {jackpotChance * 100:F4}%";
        _magnificationText.text = $" 가로줄 : {magnification * 1.2}x" +
                                  $"\n 세로줄 : {magnification * 1.5}x" +
                                  $"\n 잭팟 : {magnification * 100}x" +
                                  $"\n 실패 : 0x" +
                                  $"\n 보너스 : 2x";
        _remainSpins.text = $"{_haveSpin}";
        _SpinCosts.text = $"{_spinCost}";
        _startSpinbug = false;
    }

    private void Update()
    {
        if (!isStartSpin) return;

        elapsedTime += Time.deltaTime;

        for (int col = 0; col < 5; col++)
        {
            if (!isReelSpinned[col] && elapsedTime >= spinDuration)
            {
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
    private void ApplyHorizontalMatch()
    {
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
            int value = GetRandomSymbol();
            for (int col = 0; col < 5; col++)
            {
                reelResults[row, col] = value;
            }
        }
    }


    private void ApplyJackpot()
    {
        int jackpotSymbol = GetRandomSymbol();
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
    public void OnClickpull()
    {
        ResetReels();

        string input = inputBetAmount.text.Trim().Replace(",", "");

        horizontalMatchParticle.Stop();
        if (!long.TryParse(input, out long bet) || bet < _minBet)
        {
            OnMessage(Color.red, "잘못된 베팅금입니다.");
            return;
        }

        if (credits.Money < bet)
        {
            OnMessage(Color.red, "보유한 금액이 부족합니다.");
            return;
        }

        credits.Money -= bet;
        logUI.AddLog($"-{bet.ToString("N0")}원 : 보유금 {credits.Money.ToString("N0")}원", Color.red);
        lastBetAmount = bet;   // 이번 스핀의 베팅 금액 저장
        fallChecked = false;   // Fall 체크 초기화

        UpdateMagnificationUI();
        EnoughSpin();
    }
    public void EnoughSpin()
    {

        if (_haveSpin <= 0 || _haveSpin < _spinCost)
        {
            ButtonFlase();
            OnMessage(Color.white, "보유한 티켓이 부족합니다.");
            return;
        }
        else
        {
            if (_startSpinbug) return;
            StartSpin();
            _haveSpin -= _spinCost;
            UpdateMagnificationUI();
        }

    }
    public void ButtonTrue()
    {
        pullButton.interactable = true;
        minBetButton.interactable = true;
        maxBetButton.interactable = true;
    }

    public void ButtonFlase()
    {
        pullButton.interactable = false;
        minBetButton.interactable = false;
        maxBetButton.interactable = false;
    }

    public void OnSpinP()
    {
        if (credits.Money < _spinCoststandard)
        {
            OnMessage(Color.white, "보유한 금액이 부족합니다.");
            return;
        }

        credits.Money -= _spinCoststandard;
        _haveSpin += 1;
        ButtonTrue();
        textCredits.text = $"보유 금액 : {credits.Money.ToString("N0")}원";
        UpdateMagnificationUI();
    }

    public void OnClickP()
    {
        if (_haveSpin < 1)
        {
            OnMessage(Color.white, "보유한 티켓이 부족합니다.");
            return;
        }
        _haveSpin -= 1;
        _spinCost = Mathf.Clamp(_spinCost += 2, 1, 10);
        magnification = Mathf.Clamp(magnification + 1, 1, 10);

        UpdateMagnificationUI();
    }

    public void OnClickM()
    {
        if (_haveSpin < 1)
        {
            OnMessage(Color.white, "보유한 티켓이 부족합니다.");
            return;
        }
        _spinCost = Mathf.Clamp(_spinCost -= 2, 1, 10);
        magnification = Mathf.Clamp(magnification - 1, 1, 10);

        UpdateMagnificationUI();
    }

    public void UpdateMagnificationUI()
    {
        // 버튼 상태 갱신
        mButton.interactable = magnification > 1;
        pButton.interactable = magnification < 10;

        if (magnification <= 1)
            _magnificationText.text = $" 세로줄 : {magnification * 1.2}x" +
                                      $"\n 가로줄 : {magnification * 1.5}x" +
                                      $"\n 잭팟 : {magnification * 100}x" +
                                      $"\n 실패 : 0x" +
                                      $"\n 보너스 : 2x";
        else if (magnification == 2)
            _magnificationText.text =
                                   $" 세로줄 : {magnification * 1.2}x" +
                                   $"\n 가로줄 : {magnification * 1.5}x" +
                                   $"\n 잭팟 : {magnification * 100}x" +
                                   $"\n 실패 : {magnification * 2}x" +
                                   $"\n 보너스 : 2x";
        else if (magnification >= 3)
            _magnificationText.text =
                                   $" 세로줄 : {magnification * 1.2}x" +
                                   $"\n 가로줄 : {magnification * 1.5}x" +
                                   $"\n 잭팟 : {magnification * 100}x" +
                                   $"\n 실패 : {magnification * 5}x" +
                                   $"\n 보너스 : 2x";
        if (_haveSpin == 777 && credits.Money == 777000)
            _magnificationText.text =
                                  $" 세로줄 : {magnification * 1.2}x" +
                                  $"\n 가로줄 : {magnification * 1.5}x" +
                                  $"\n 잭팟 : {magnification * 100}x" +
                                  $"\n 실패 : {magnification * 0}x" +
                                  $"\n 보너스 : 7x";

        textCredits.text = $"보유 금액 : {credits.Money:N0}원";
        _remainSpins.text = $"{_haveSpin}";
        _SpinCosts.text = $"{_spinCost}";
    }

    private void StartSpin()
    {
        isStartSpin = true;
        ButtonFlase();
        ResetReelSpins();

        // 0) 항상 전체 기본 랜덤 채우기
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                reelResults[r, c] = GetRandomSymbol();

        // 1) 패턴 결정
        SpinPattern pattern = DecidePattern();

        // 2) 패턴 오버레이
        switch (pattern)
        {
            case SpinPattern.Jackpot:
                ApplyJackpot();
                break;

            case SpinPattern.Horizontal:
                ApplyHorizontalMatch();
                jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
                break;

            case SpinPattern.Vertical:
                int col = UnityEngine.Random.Range(0, 5);
                ForceVerticalColumn(col);  // 아래 새 함수 사용
                jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
                break;

            case SpinPattern.Normal:
                jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
                break;
        }

        // 3) 스핀 코루틴 시작
        for (int c = 0; c < 5; c++)
        {
            if (reelSpinCoroutines[c] != null) StopCoroutine(reelSpinCoroutines[c]);
            reelSpinCoroutines[c] = StartCoroutine(SpinReelLoop(c));
        }

        StartCoroutine(StopReelsOneByOne());
    }


    private SpinPattern DecidePattern()
    {

        float r = UnityEngine.Random.value;
        float pJ = jackpotChance;
        float pH = _horizontalChance;
        float pV = _verticalChance;

        if (r < pJ) return SpinPattern.Jackpot;
        r -= pJ;
        if (r < pH) return SpinPattern.Horizontal;
        r -= pH;
        if (r < pV) return SpinPattern.Vertical;
        return SpinPattern.Normal;
    }
    private void ForceVerticalColumn(int col)
    {
        int v = GetRandomSymbol();
        for (int row = 0; row < 3; row++)
            reelResults[row, col] = v;
    }



    public void OnClickMinimumbet()
    {
        if (credits.Money <= 0)
        {
            OnMessage(Color.red, "보유한 금액이 부족합니다.");
            return;
        }

        inputBetAmount.text = _minBet.ToString();
        OnClickpull();
    }
    public void OnClickMaximumbet()
    {
        inputBetAmount.text = credits.Money.ToString();
        OnClickpull();
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
                int randVal = GetRandomSymbol();
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
        if (isStartSpin)
            CheckBet();
        if (_haveSpin > 0)
        {
            ButtonTrue();
        }
    }

    public IEnumerator PlayHorizontalMatchEffects()
    {
        if (_this == true) horizontalMatchParticle.Play();
        else horizontalMatchParticle.Stop();

        // 카메라 + UI 동시에 흔들기
        yield return StartCoroutine(CameraAndMultipleUICanvasShake(0.5f, 0.05f, 5f));
    }
    private IEnumerator CameraAndMultipleUICanvasShake(float duration, float camMagnitude, float uiMagnitude)
    {
        Vector3 camOriginal = cameraTransform.localPosition;
        Vector2[] uiOriginals = new Vector2[uiCanvases.Length];

        // 각 UI Canvas 원위치 저장 & LayoutGroup 잠시 비활성화
        for (int i = 0; i < uiCanvases.Length; i++)
        {
            uiOriginals[i] = uiCanvases[i].anchoredPosition;
            if (layoutGroups != null && layoutGroups.Length > i && layoutGroups[i] != null)
                layoutGroups[i].enabled = false;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // 카메라 흔들기
            float camX = UnityEngine.Random.Range(-1f, 1f) * camMagnitude;
            float camY = UnityEngine.Random.Range(-1f, 1f) * camMagnitude;
            cameraTransform.localPosition = camOriginal + new Vector3(camX, camY, 0);

            // 각 UI Canvas 흔들기
            for (int i = 0; i < uiCanvases.Length; i++)
            {
                float uiX = UnityEngine.Random.Range(-1f, 1f) * uiMagnitude;
                float uiY = UnityEngine.Random.Range(-1f, 1f) * uiMagnitude;
                uiCanvases[i].anchoredPosition = uiOriginals[i] + new Vector2(uiX, uiY);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원위치 복원 & LayoutGroup 재활성화
        cameraTransform.localPosition = camOriginal;
        for (int i = 0; i < uiCanvases.Length; i++)
        {
            uiCanvases[i].anchoredPosition = uiOriginals[i];
            if (layoutGroups != null && layoutGroups.Length > i && layoutGroups[i] != null)
                layoutGroups[i].enabled = true;
        }
    }

    #endregion
    private void OnMessage(Color color, string msg)
    {
        imageBetAmount.color = color;
        textResult.text = msg;
    }
    public bool hasMatch { get; set; }
    [SerializeField] private GameObject bag;
    private void CheckBet()
    {
        hasMatch = false; // 외부에서 불려오기 위해 밖에서 선언 하고 프로퍼티함 - 박철민

        foreach (var img in reelImagesFlat)
            img.color = Color.white;

        if (CheckJackpot(lastBetAmount))
            return;

        bool vertical = CheckVertical(lastBetAmount);
        bool horizontal = CheckHorizontal(lastBetAmount);
        bool jackpot = CheckJackpot(lastBetAmount);
        hasMatch = vertical || horizontal;

        if (_minBet == 0)
            _minBet += 1;

        if (credits.Money >= long.MaxValue / 2)
            CreditMaxOver();

        if (!hasMatch)
        {
            Fall();
        }

        _minBetText.text = $"최소 베팅금 : {_minBet.ToString("N0")}원";
        textCredits.text = $"보유 금액 : {credits.Money.ToString("N0")}원";
        textChance.text = $" 세로줄 : {_verticalChance * 100}% \n 가로줄 : {_horizontalChance * 100}% \n 잭팟 : {jackpotChance * 100:F4}%";
        foreach (var item in items)
        {
            Debug.Log("들어옴");
            if (!item.transform.IsChildOf(bag.transform))
                continue;

            Debug.Log($"Invoke 시도: {item.name}");
            var itemOn = item.GetComponent<ItemOn>();
            itemOn.OnAbilityCast?.Invoke();
        }

        textResult.text = hasMatch ? "성공!!!" : "실패!!!!";

        if (horizontal || jackpot)
        {
            _this = true;
            StartCoroutine(PlayHorizontalMatchEffects());
            _this = false;
        }

        // 아마도 내가 추가함 - 박철민

    }
    private bool CheckVertical(long bet)
    {
        bool matched = false;
        float aa = 1.2f;

        for (int col = 0; col < 5; col++)
        {
            int a = reelResults[0, col];
            int b = reelResults[1, col];
            int c = reelResults[2, col];

            if (a == b && b == c)
            {
                long reward = (long)(bet * (magnification * aa));
                matched = true;
                if (a == 6)
                {
                    reward = -reward;
                }
                if (a == 7)
                {
                    if (_haveSpin == 777 && credits.Money == 777000)
                        reward *= 7;
                    else
                        reward *= 2;
                    textResult.text = "777 보너스!!! ";
                }
                AddCredits(reward);

                for (int row = 0; row < 3; row++)
                {
                    reelImages[row, col].color = customMatch;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.1f));
                }
            }
        }

        return matched;
    }

    private bool CheckHorizontal(long bet)
    {
        bool matched = false;
        float aa = 1.5f;
        for (int row = 0; row < 3; row++)
        {
            int a = reelResults[row, 0];
            int b = reelResults[row, 1];
            int c = reelResults[row, 2];
            int d = reelResults[row, 3];
            int e = reelResults[row, 4];

            if (a == b && b == c && c == d && d == e)
            {
                long reward = (long)(bet * (magnification * aa));
                matched = true;
                if (a == 6)
                {
                    reward = -reward;
                }
                if (a == 7)
                {
                    if (_haveSpin == 777 && credits.Money == 777000)
                        reward *= 7;
                    else
                        reward *= 2;
                    textResult.text = "777 보너스!!! ";
                }
                NoBagDouble noBag = FindAnyObjectByType<NoBagDouble>();
                if (noBag != null)
                {
                    if (noBag.Nobagdouble())
                    {
                        reward *= 2;
                        Debug.Log("된다");
                    }
                }
                AddCredits(reward);
                for (int col = 0; col < 5; col++)
                {
                    reelImages[row, col].color = customMatch;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.1f));
                }
            }
        }

        return matched;
    }

    public bool CheckJackpot(long betAmount)
    {
        int first = reelResults[0, 0];

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                if (reelResults[r, c] != first)
                    return false;


        long reward = betAmount * (magnification * 100);
        jackpotChance = jackpotChanceInitial;
        if (first == 6)
        {
            reward = -reward;
        }
        if (first == 7)
        {
            if (_haveSpin == 777 && credits.Money == 777000)
                reward *= 777;
            else
                reward *= 2;
            textResult.text = " 잭팟 777 보너스!!! ";
        }
        else
        {
            textResult.text = " 잭팟!!! ";
        }
        AddCredits(reward);
        textCredits.text = $"보유 금액 : {credits.Money.ToString("N0")}원";

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
            {
                reelImages[r, c].color = customJackPot;
                StartCoroutine(BlinkText(reelTexts[r, c], 0.5f, 0.15f));
            }
        return true;
    }

    private bool Fall()
    {
        if (fallChecked) return false; // 이미 체크했으면 중복 방지
        fallChecked = true;
        long aa = lastBetAmount;
        if (magnification <= 1)
            aa *= -(magnification * 0);
        else if (magnification == 2)
            aa *= -(magnification * 2);
        else if (magnification >= 3)
            aa *= -(magnification * 5);

        if (magnification > 1)
            AddCredits(aa);

        credits.Money = Math.Clamp(credits.Money, 0, long.MaxValue / 2);
        if (credits.Money <= 0)
        {
            credits.Money = 0;
        }
        return true;
    }

    private void AddCredits(long amount)
    {
        try
        {
            checked
            {
                credits.Money += amount;
            }
        }
        catch (OverflowException)
        {
            credits.Money = long.MaxValue / 2; // 상한으로 고정
        }

        credits.Money = Math.Clamp(credits.Money, 0, long.MaxValue / 2);
        if (amount > 0)
        {
            logUI.AddLog($"+{amount.ToString("N0")}원 : 보유금 {credits.Money.ToString("N0")}원", Color.green);
        }
        else
        {
            logUI.AddLog($"-{amount.ToString("N0")}원 : 보유금 {credits.Money.ToString("N0")}원", Color.red);
        }
    }

    private void CreditMaxOver()
    {
        credits.Money = long.MaxValue / 2;
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
    private int GetRandomSymbol()
    {
        int rand = UnityEngine.Random.Range(0, 100); // 0~99 사이 정수

        if (rand < 20) return 1;
        else if (rand < 40) return 2;
        else if (rand < 60) return 3;
        else if (rand < 75) return 4;
        else if (rand < 95) return 5;
        else if (rand < 99) return 6;
        else return 7; // 1% 확률
    }
}
