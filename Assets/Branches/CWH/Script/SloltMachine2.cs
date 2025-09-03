using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SloltMachine2 : MonoBehaviour
{
    public enum SpinPattern
    {
        Jackpot, Horizontal, Vertical, Normal
    }
    //���� ���ɿ��ڵ�� �ӽ÷� ����� -����
    #region ���� ���ɿ� �ڵ� 
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

    private long lastBetAmount;
    private bool fallChecked;

    [Header("��")]
    [SerializeField] private MoneyManager credits;
    [SerializeField] private long _startCredits;

    [SerializeField] private TMP_InputField inputBetAmount;
    [SerializeField] private Image imageBetAmount;
    [SerializeField] private TextMeshProUGUI textCredits;
    [SerializeField] private TextMeshProUGUI _minBetText;
    [SerializeField] private long _minBet;
    [SerializeField] private long _maxBet;

    [Header("�� �ؽ�Ʈ//���ӿ� ���̴� ��")]
    [SerializeField] private TextMeshProUGUI[] reelTextsFlat = new TextMeshProUGUI[15];

    [Header("�� �̹���//���ӿ� ���̴� ��")]
    [SerializeField] private Image[] reelImagesFlat = new Image[15];

    [Header("ī�޶�")]
    [SerializeField] private Transform cameraTransform;

    [Header("��ƼŬ")]
    [SerializeField] private ParticleSystem horizontalMatchParticle;

    [Header("���� ����")]
    [SerializeField] private int magnification;
    [SerializeField] private TextMeshProUGUI _magnificationText;

    [Header("���� ���� �� (����)")]
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

    #region ����Ȯ�� ����
    [Header("����")]
    [SerializeField] private float jackpotChance = 0.00001f;
    private const float jackpotChanceMax = 0.5f;
    private const float jackpotChanceIncrement = 0.0000001f;
    private const float jackpotChanceInitial = 0.000000005f;

    #endregion
    [Header("����")]
    [field: SerializeField] public float _verticalChance;
    public float VerticalChance { get; set; }
    [Header("����")]
    [field: SerializeField] public float _horizontalChance;
    public float HorizonTalChance { get; set; }

    //�ؽ�Ʈ, ��ư
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textChance;
    public Button pullButton;
    public Button minBetButton;
    public Button maxBetButton;
    public Button pButton;
    public Button mButton;

    //�� ���������� ���ư��� ��
    private int[,] reelResults = new int[3, 5];
    private Image[,] reelImages = new Image[3, 5];
    private TextMeshProUGUI[,] reelTexts = new TextMeshProUGUI[3, 5];

    //���� ���ư��½ð�,��� �ð� , ���� ���� ����
    private float spinDuration = 0.2f;
    private float elapsedTime = 0f;
    private bool isStartSpin = false;

    //�� ���� ���󰡴� ��
    private Coroutine[] reelSpinCoroutines = new Coroutine[5];
    //�� ���� ����
    private bool[] isReelSpinned = new bool[5];

    //�����
    Color32 customMatch = new Color32(255, 239, 184, 255);
    Color32 customJackPot = new Color32(207, 255, 182, 255);

    bool _startSpinbug = true;

    private void Start()
    {
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
        textCredits.text = $"Credits : {credits.Money.ToString("N0")}";
        _minBetText.text = $"Minbet : {_minBet.ToString("N0")}";
        textChance.text = $" Vertical : {_verticalChance * 100}% \n Horizontal : {_horizontalChance * 100}% \n Jackpot : {jackpotChance * 100:F4}%";
        _magnificationText.text = $" Vertical : {magnification * 2}x" +
                                  $"\n Horizontal : {magnification * 4}x" +
                                  $"\n Jackpot : {magnification * 1000}x" +
                                  $"\n Fall : 0x" +
                                  $"\n Bonus : 2x";
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
        int matchRowCount = GetRandomSymbol(); // 1~2�� ��Ī
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
            OnMessage(Color.red, "Invalid bet amount");
            return;
        }

        if (credits.Money < bet)
        {
            OnMessage(Color.red, "You don't have enough money");
            return;
        }

        credits.Money -= bet;
        lastBetAmount = bet;   // �̹� ������ ���� �ݾ� ����
        fallChecked = false;   // Fall üũ �ʱ�ȭ

        UpdateMagnificationUI();
        EnoughSpin();
    }

    public void EnoughSpin()
    {

        if (_haveSpin <= 0 || _haveSpin < _spinCost)
        {
            pullButton.interactable = false;
            minBetButton.interactable = false;
            maxBetButton.interactable = false;
            OnMessage(Color.white, "You don't have enough Spin");
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

    public void OnSpinP()
    {
        if (credits.Money < _spinCoststandard)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }

        credits.Money -= _spinCoststandard;
        _haveSpin += 1;
        pullButton.interactable = true;
        minBetButton.interactable = true;
        maxBetButton.interactable = true;
        textCredits.text = $"Credits : {credits.Money.ToString("N0")}";
        UpdateMagnificationUI();
    }

    public void OnClickP()
    {
        if (credits.Money < 10)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }
        credits.Money -= magnification * magnification;
        _spinCost = Mathf.Clamp(_spinCost += 2, 1, 10);
        magnification = Mathf.Clamp(magnification + 1, 1, 10);

        UpdateMagnificationUI();
    }

    public void OnClickM()
    {
        if (credits.Money < 10)
        {
            OnMessage(Color.white, "You don't have enough money");
            return;
        }
        credits.Money -= magnification * 2;
        _spinCost = Mathf.Clamp(_spinCost -= 2, 1, 10);
        magnification = Mathf.Clamp(magnification - 1, 1, 10);

        UpdateMagnificationUI();
    }

    private void UpdateMagnificationUI()
    {
        // ��ư ���� ����
        mButton.interactable = magnification > 1;
        pButton.interactable = magnification < 10;

        if (magnification <= 1)
            _magnificationText.text = $" Vertical : {magnification * 2}x" +
                                      $"\n Horizontal : {magnification * 4}x" +
                                      $"\n Jackpot : {magnification * 1000}x" +
                                      $"\n Fall : 0x" +
                                      $"\n Bonus : 2x";
        else if (magnification == 2)
            _magnificationText.text =
                                   $" Vertical : {magnification * 2}x" +
                                   $"\n Horizontal : {magnification * 4}x" +
                                   $"\n Jackpot : {magnification * 1000}x" +
                                   $"\n Fall : {magnification * 2}x" +
                                   $"\n Bonus : 2x";
        else if (magnification >= 3)
            _magnificationText.text =
                                   $" Vertical : {magnification * 2}x" +
                                   $"\n Horizontal : {magnification * 4}x" +
                                   $"\n Jackpot : {magnification * 1000}x" +
                                   $"\n Fall : {magnification * 5}x" +
                                   $"\n Bonus : 2x";
        if (_haveSpin == 777 && credits.Money == 777000)
            _magnificationText.text =
                                  $" Vertical : {magnification * 2}x" +
                                  $"\n Horizontal : {magnification * 4}x" +
                                  $"\n Jackpot : {magnification * 1000}x" +
                                  $"\n Fall : {magnification * 0}x" +
                                  $"\n Bonus : 7x";

        textCredits.text = $"Credits : {credits.Money:N0}";
        _remainSpins.text = $"{_haveSpin}";
        _SpinCosts.text = $"{_spinCost}";
    }

    private void StartSpin()
    {
        isStartSpin = true;
        pullButton.interactable = false;
        minBetButton.interactable = false;
        maxBetButton.interactable = false;
        ResetReelSpins();

        // 0) �׻� ��ü �⺻ ���� ä���
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                reelResults[r, c] = GetRandomSymbol();

        // 1) ���� ����
        SpinPattern pattern = DecidePattern();

        // 2) ���� ��������
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
                ForceVerticalColumn(col);  // �Ʒ� �� �Լ� ���
                jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
                break;

            case SpinPattern.Normal:
                jackpotChance = Mathf.Min(jackpotChance + jackpotChanceIncrement, jackpotChanceMax);
                break;
        }

        // 3) ���� �ڷ�ƾ ����
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
            OnMessage(Color.red, "You have no credits");
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

        if (_minBet == 0)
            _minBet += 1;

        if (credits.Money >= long.MaxValue / 2)
            CreditMaxOver();

        if (!hasMatch)
        {
            Fall();
        }
        _minBetText.text = $"Minbet : {_minBet.ToString("N0")}";
        textCredits.text = $"Credits : {credits.Money.ToString("N0")}";
        textChance.text = $" Vertical : {_verticalChance * 100}% \n Horizontal : {_horizontalChance * 100}% \n Jackpot : {jackpotChance * 100:F4}%";
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

    #region �ڷ�ƾ
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

        // ���� ��� ǥ��
        for (int row = 0; row < 3; row++)
        {
            reelTexts[row, col].text = reelResults[row, col].ToString("D1");
        }
    }
    private IEnumerator StopReelsOneByOne()
    {
        for (int col = 0; col < 5; col++)
        {
            yield return new WaitForSeconds(0.2f); // �� �� ���ߴ� ����
            isReelSpinned[col] = true;             // �� �� ����
        }

        yield return new WaitForSeconds(0.2f);

        isStartSpin = false;
        if (isStartSpin)
            CheckBet();
        if (_haveSpin > 0)
        {
            pullButton.interactable = true;
            minBetButton.interactable = true;
            maxBetButton.interactable = true;
        }
    }

    private IEnumerator PlayHorizontalMatchEffects()
    {
        // ��ƼŬ ��� (��: particleSystem.Play();)
        horizontalMatchParticle.Play();

        // ȭ�� ���� ȿ�� ����
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
        float aa = 1.2f;

        for (int col = 0; col < 5; col++)
        {
            int a = reelResults[0, col];
            int b = reelResults[1, col];
            int c = reelResults[2, col];

            if (a == b && b == c)
            {
                long reward =(long)(  bet * (magnification * aa));
                matched = true;
                if (a == 7)
                {
                    if (_haveSpin == 777 && credits.Money == 777000)
                        reward *= 7;
                    else
                        reward *= 2;
                    textResult.text = "777 BONUS!!! ";
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

        for (int row = 0; row < 3; row++)
        {
            int a = reelResults[row, 0];
            int b = reelResults[row, 1];
            int c = reelResults[row, 2];
            int d = reelResults[row, 3];
            int e = reelResults[row, 4];

            if (a == b && b == c && c == d && d == e)
            {
                long reward = bet * (magnification * 4);
                matched = true;
                if (a == 7)
                {
                    if (_haveSpin == 777 && credits.Money == 777000)
                        reward *= 7;
                    else
                        reward *= 2;
                    textResult.text = "777 BONUS!!! ";
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

    private bool CheckJackpot(long betAmount)
    {
        int first = reelResults[0, 0];

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                if (reelResults[r, c] != first)
                    return false;


        long reward = betAmount * (magnification * 1000);
        jackpotChance = jackpotChanceInitial;

        if (first == 7)
        {
            if (_haveSpin == 777 && credits.Money == 777000)
                reward *= 7;
            else
                reward *= 2;
            textResult.text = " JACKPOT 777 BONUS!!! ";
        }
        else
        {
            textResult.text = " JACKPOT!!! ";
        }
        AddCredits(reward);
        textCredits.text = $"Credits : {credits.Money.ToString("N0")}";

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
        if (fallChecked) return false; // �̹� üũ������ �ߺ� ����
        fallChecked = true;

        if (magnification <= 1)
            credits.Money -= lastBetAmount * (magnification * 0);
        else if (magnification == 2)
            credits.Money -= lastBetAmount * (magnification * 2);
        else if (magnification >= 3)
            credits.Money -= lastBetAmount * (magnification * 5);

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
            credits.Money = long.MaxValue / 2; // �������� ����
        }

        credits.Money = Math.Clamp(credits.Money, 0, long.MaxValue / 2);
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
        int rand = UnityEngine.Random.Range(0, 100); // 0~99 ���� ����

        if (rand < 20) return 1;
        else if (rand < 40) return 2;
        else if (rand < 60) return 3;
        else if (rand < 75) return 4;
        else if (rand < 95) return 5;
        else if (rand < 98) return 6;
        else return 7; // 10% Ȯ��
    }
}
