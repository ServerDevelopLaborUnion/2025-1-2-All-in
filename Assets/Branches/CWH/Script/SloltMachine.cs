using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SloltMachine : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBetAmount;
    [SerializeField] private Image imageBetAmount;
    [SerializeField] private TextMeshProUGUI textCredits;

    [Header("릴 텍스트")]
    [SerializeField] private TextMeshProUGUI[] reelTextsFlat = new TextMeshProUGUI[15];

    [Header("릴 이미지")]
    [SerializeField] private Image[] reelImagesFlat = new Image[15];


    [Header("카메라")]
    [SerializeField] private Transform cameraTransform;


    [Header("파티클")]
    [SerializeField] private ParticleSystem horizontalMatchParticle;

    #region 잭팟확률 관련
    private float jackpotChance = 0.05f;
    private const float jackpotChanceMax = 1f;
    private const float jackpotChanceIncrement = 0.005f;
    private const float jackpotChanceInitial = 0.05f;

    #endregion
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textChance;
    [SerializeField] private Button pullButton;
    [SerializeField] private Button allInButton;
    private Coroutine[] reelSpinCoroutines = new Coroutine[5];

    private int[,] reelResults = new int[3, 5];
    private Image[,] reelImages = new Image[3, 5];
    private TextMeshProUGUI[,] reelTexts = new TextMeshProUGUI[3, 5];

    private float spinDuration = 0.2f;
    private float elapsedTime = 0f;
    private bool isStartSpin = false;
    private bool isHorizontalMatchApplied = false;


    private int credits = 100000;

    private bool[] isReelSpinned = new bool[5];

    Color32 customJackPot = new Color32(255, 239, 184, 255);

    private void Awake()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                reelImages[row, col] = reelImagesFlat[row * 5 + col];
                reelTexts[row, col] = reelTextsFlat[row * 5 + col];
            }
        }

        textCredits.text = $"Credits : {credits}";
        textChance.text = $"Jackpot Chance \n {jackpotChance:F3}%";

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

            if (Random.value < 0.1f)
                ApplyHorizontalMatch();

            UpdateReelDisplay();
            CheckBet();
            pullButton.interactable = true;
        }
    }

    private void ApplyVerticalMatch(int col)
    {
        int baseSpin = Random.Range(1, 8);
        bool forceVerticalMatch = Random.value < 0.2f;

        for (int row = 0; row < 3; row++)
        {
            reelResults[row, col] = forceVerticalMatch ? baseSpin : Random.Range(1, 8);
        }
    }

    private void ApplyHorizontalMatch()
    {
        isHorizontalMatchApplied = false; // 초기화

        int matchRowCount = Random.Range(1, 4); // 1~2줄 매칭
        List<int> rows = new List<int> { 0, 1, 2 };
        for (int i = 0; i < rows.Count; i++)
        {
            int j = Random.Range(i, rows.Count);
            (rows[i], rows[j]) = (rows[j], rows[i]);
        }

        for (int i = 0; i < matchRowCount; i++)
        {
            int row = rows[i];
            int value = Random.Range(1, 8);
            for (int col = 0; col < 5; col++)
            {
                reelResults[row, col] = value;
            }
            isHorizontalMatchApplied = true;  // 가로매치 적용됨 표시
        }
    }

    private void ApplyJackpot()
    {
        int jackpotSymbol = Random.Range(1, 8);

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
        textCredits.text = $"Credits : {credits}";
    }

    public void OnClickpull()
    {
        ResetReels();
        horizontalMatchParticle.Stop();
        if (!int.TryParse(inputBetAmount.text.Trim(), out int bet) || bet <= 0)
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
        textCredits.text = $"Credits : {credits}";
    

        StartSpin();
        pullButton.interactable = false;
    }

    private void StartSpin()
    {
        isStartSpin = true;
        pullButton.interactable = false;
        elapsedTime = 0;
        ResetReelSpins();

        // 기본 랜덤 결과 생성
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 5; col++)
                reelResults[row, col] = Random.Range(1, 8);

        // 세로줄 매치 확률 적용
        for (int col = 0; col < 5; col++)
        {
            if (Random.value < 0.1f)
            {
                int val = Random.Range(1, 8);
                for (int row = 0; row < 3; row++)
                    reelResults[row, col] = val;
            }
        }

        float rand = Random.value;

        if (rand < jackpotChance)
        {
            ApplyJackpot();
            
        }
        else if (rand < jackpotChance + 0.5f)
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
    public void OnClickAllIn()
    {
        if (credits <= 0)
        {
            OnMessage(Color.red, "You have no credits to All-In");
            return;
        }

        inputBetAmount.text = credits.ToString();
        OnClickpull();
    }

    private void CheckBet()
    { 
        int betAmount = int.Parse(inputBetAmount.text);
        bool hasMatch = false;

        foreach (var img in reelImagesFlat)
            img.color = Color.white;

        if (CheckJackpot(betAmount))
            return;

        bool vertical = CheckVertical(betAmount);
        bool horizontal = CheckHorizontal(betAmount);
        bool jackpot = CheckJackpot(betAmount);
        hasMatch = vertical || horizontal;

        textCredits.text = $"Credits : {credits}";
        textChance.text = $"Jackpot Chance \n {jackpotChance:F3}%";
        textResult.text = hasMatch ? "YOU WIN!!!" : "YOU LOSE!!!!";


        if (horizontal||jackpot)
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

    private IEnumerator StartSpinSequence()
    {
        isStartSpin = true;
        pullButton.interactable = false;

        ResetReelSpins();

        // 결과 랜덤 생성(필요시)
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 5; col++)
                reelResults[row, col] = Random.Range(1, 8);

        for (int col = 0; col < 5; col++)
        {
            if (Random.value < 0.3f)
            {
                int val = Random.Range(1, 8);
                for (int row = 0; row < 3; row++)
                    reelResults[row, col] = val;
            }
        }

        if (Random.value < 0.5f)
            ApplyHorizontalMatch();

        for (int col = 0; col < 5; col++)
        {
            yield return StartCoroutine(SpinReelCoroutine(col)); // 한 릴씩 순차적으로 스핀 & 멈춤
        }

        isStartSpin = false;

        yield return PlayHorizontalMatchEffects();

        CheckBet();
        pullButton.interactable = true;
    }

    private IEnumerator SpinReelCoroutine(int col)
    {
        float spinTime = 0.8f; // 릴당 스핀 시간 조절
        float elapsed = 0f;
        float interval = 0.05f;

        while (elapsed < spinTime)
        {
            for (int row = 0; row < 3; row++)
            {
                int randVal = Random.Range(1, 8);
                reelTexts[row, col].text = randVal.ToString();
            }
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        // 최종 결과 표시
        for (int row = 0; row < 3; row++)
        {
            reelTexts[row, col].text = reelResults[row, col].ToString("D1");
        }

        isReelSpinned[col] = true;
    }


    private IEnumerator SpinReelLoop(int col)
    {
        while (!isReelSpinned[col])
        {
            for (int row = 0; row < 3; row++)
            {
                int randVal = Random.Range(1, 8);
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
    }

    private IEnumerator PlayHorizontalMatchEffects()
    {
        if (isHorizontalMatchApplied)
        {
            // 파티클 재생 (예: particleSystem.Play();)
            horizontalMatchParticle.Play();

            // 화면 흔들기 효과 실행
            yield return StartCoroutine(ScreenShakeCoroutine(0.5f, 0.01f));
        }
    }

    private IEnumerator ScreenShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

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

    private bool CheckVertical(int bet)
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
                credits += bet * 2;

                for (int row = 0; row < 3; row++)
                {
                    reelImages[row, col].color = customJackPot;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.2f, 0.15f));
                }
            }
        }

        return matched;
    }

    private bool CheckHorizontal(int bet)
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
                credits += bet * 4;

                for (int col = 0; col < 5; col++)
                {
                    reelImages[row, col].color = customJackPot;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.15f));
                }
            }
        }

        return matched;
    }

    private bool CheckJackpot(int betAmount)
    {
        int first = reelResults[0, 0];

        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 5; c++)
                if (reelResults[r, c] != first)
                    return false;

        jackpotChance = jackpotChanceInitial;
        // 잭팟 처리
        textResult.text = " JACKPOT!!! ";
        credits += betAmount * 100;
        textCredits.text = $"Credits : {credits}";
        return true;
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
