using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SloltMachine : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBetAmount;
    [SerializeField] private Image imageBetAmount;

    #region 릴
    [SerializeField] private TextMeshProUGUI textCredits;
    [SerializeField] private TextMeshProUGUI text11Reel;
    [SerializeField] private TextMeshProUGUI text12Reel;
    [SerializeField] private TextMeshProUGUI text13Reel;
    [SerializeField] private TextMeshProUGUI text14Reel;
    [SerializeField] private TextMeshProUGUI text15Reel;
    [SerializeField] private TextMeshProUGUI text21Reel;
    [SerializeField] private TextMeshProUGUI text22Reel;
    [SerializeField] private TextMeshProUGUI text23Reel;
    [SerializeField] private TextMeshProUGUI text24Reel;
    [SerializeField] private TextMeshProUGUI text25Reel;
    [SerializeField] private TextMeshProUGUI text31Reel;
    [SerializeField] private TextMeshProUGUI text32Reel;
    [SerializeField] private TextMeshProUGUI text33Reel;
    [SerializeField] private TextMeshProUGUI text34Reel;
    [SerializeField] private TextMeshProUGUI text35Reel;
    [SerializeField] private TextMeshProUGUI textResult;
    #endregion

    #region 릴 상태
    private bool is1ReelSpinned = false;
    private bool is2ReelSpinned = false;
    private bool is3ReelSpinned = false;
    private bool is4ReelSpinned = false;
    private bool is5ReelSpinned = false;
    #endregion

    #region 릴 이미지
    [SerializeField] private Image f1ReelImage;
    [SerializeField] private Image f2ReelImage;
    [SerializeField] private Image f3ReelImage;
    [SerializeField] private Image f4ReelImage;
    [SerializeField] private Image f5ReelImage;

    [SerializeField] private Image s1ReelImage;
    [SerializeField] private Image s2ReelImage;
    [SerializeField] private Image s3ReelImage;
    [SerializeField] private Image s4ReelImage;
    [SerializeField] private Image s5ReelImage;

    [SerializeField] private Image t1ReelImage;
    [SerializeField] private Image t2ReelImage;
    [SerializeField] private Image t3ReelImage;
    [SerializeField] private Image t4ReelImage;
    [SerializeField] private Image t5ReelImage;
    #endregion

    #region 릴의 결과값
    private int f1ReelResult = 0;
    private int f2ReelResult = 0;
    private int f3ReelResult = 0;
    private int f4ReelResult = 0;
    private int f5ReelResult = 0;

    private int s1ReelResult = 0;
    private int s2ReelResult = 0;
    private int s3ReelResult = 0;
    private int s4ReelResult = 0;
    private int s5ReelResult = 0;

    private int t1ReelResult = 0;
    private int t2ReelResult = 0;
    private int t3ReelResult = 0;
    private int t4ReelResult = 0;
    private int t5ReelResult = 0;
    #endregion

    private int[,] reelResults = new int[3, 5];
    private Image[,] reelImages = new Image[3, 5];
    private TextMeshProUGUI[,] reelTexts = new TextMeshProUGUI[3, 5];

    [SerializeField] private Button pullButton;
    [SerializeField] private Button allInButton;

    private float spinDuration = 0.2f;
    private float elapsedTime = 0f;
    private bool isStartSpin = false;
    [SerializeField] private int credits = 100000;

    Color32 customTwo = new Color32(178, 34, 34, 255);
    Color32 customJackPot = new Color32(255, 239, 184, 255);

    private void Awake()
    {
        Image[] images = new Image[]{
            f1ReelImage, f2ReelImage, f3ReelImage, f4ReelImage, f5ReelImage,
            s1ReelImage, s2ReelImage, s3ReelImage, s4ReelImage, s5ReelImage,
            t1ReelImage, t2ReelImage, t3ReelImage, t4ReelImage, t5ReelImage
        };

        TextMeshProUGUI[] texts = new TextMeshProUGUI[]
        {
            text11Reel, text12Reel, text13Reel, text14Reel, text15Reel,
            text21Reel, text22Reel, text23Reel, text24Reel, text25Reel,
            text31Reel, text32Reel, text33Reel, text34Reel, text35Reel
        };

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                reelImages[row, col] = images[row * 5 + col];
                reelTexts[row, col] = texts[row * 5 + col];
            }
        }
    }

    private void Update()
    {
        if (!isStartSpin) return;

        elapsedTime += Time.deltaTime;
        int random_spinResult1 = Random.Range(1, 10);
        int random_spinResult2 = Random.Range(1, 10);
        int random_spinResult3 = Random.Range(1, 10);

        if (!is1ReelSpinned)
        {
            f1ReelResult = random_spinResult1;
            s1ReelResult = random_spinResult2;
            t1ReelResult = random_spinResult3;
            if (elapsedTime >= spinDuration)
            {
                is1ReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!is2ReelSpinned)
        {
            f2ReelResult = random_spinResult1;
            s2ReelResult = random_spinResult2;
            t2ReelResult = random_spinResult3;
            if (elapsedTime >= spinDuration)
            {
                is2ReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!is3ReelSpinned)
        {
            f3ReelResult = random_spinResult1;
            s3ReelResult = random_spinResult2;
            t3ReelResult = random_spinResult3;
            if (elapsedTime >= spinDuration)
            {
                is3ReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!is4ReelSpinned)
        {
            f4ReelResult = random_spinResult1;
            s4ReelResult = random_spinResult2;
            t4ReelResult = random_spinResult3;
            if (elapsedTime >= spinDuration)
            {
                is4ReelSpinned = true;
                elapsedTime = 0;
            }
        }
        else if (!is5ReelSpinned)
        {
            f5ReelResult = random_spinResult1;
            s5ReelResult = random_spinResult2;
            t5ReelResult = random_spinResult3;
            if (elapsedTime >= spinDuration)
            {
                isStartSpin = false;
                elapsedTime = 0;

                is1ReelSpinned = false;
                is2ReelSpinned = false;
                is3ReelSpinned = false;
                is4ReelSpinned = false;
                is5ReelSpinned = false;

                //  여기서 결과값 저장
                reelResults[0, 0] = f1ReelResult;
                reelResults[0, 1] = f2ReelResult;
                reelResults[0, 2] = f3ReelResult;
                reelResults[0, 3] = f4ReelResult;
                reelResults[0, 4] = f5ReelResult;

                reelResults[1, 0] = s1ReelResult;
                reelResults[1, 1] = s2ReelResult;
                reelResults[1, 2] = s3ReelResult;
                reelResults[1, 3] = s4ReelResult;
                reelResults[1, 4] = s5ReelResult;

                reelResults[2, 0] = t1ReelResult;
                reelResults[2, 1] = t2ReelResult;
                reelResults[2, 2] = t3ReelResult;
                reelResults[2, 3] = t4ReelResult;
                reelResults[2, 4] = t5ReelResult;

                CheckBet();
                pullButton.interactable = true;
            }
        }

        text11Reel.text = f1ReelResult.ToString("D1");
        text21Reel.text = s1ReelResult.ToString("D1");
        text31Reel.text = t1ReelResult.ToString("D1");

        text12Reel.text = f2ReelResult.ToString("D1");
        text22Reel.text = s2ReelResult.ToString("D1");
        text32Reel.text = t2ReelResult.ToString("D1");

        text13Reel.text = f3ReelResult.ToString("D1");
        text23Reel.text = s3ReelResult.ToString("D1");
        text33Reel.text = t3ReelResult.ToString("D1");

        text14Reel.text = f4ReelResult.ToString("D1");
        text24Reel.text = s4ReelResult.ToString("D1");
        text34Reel.text = t4ReelResult.ToString("D1");

        text15Reel.text = f5ReelResult.ToString("D1");
        text25Reel.text = s5ReelResult.ToString("D1");
        text35Reel.text = t5ReelResult.ToString("D1");
    }

    private void ResetReels()
    {
        foreach (var img in reelImages)
            img.color = Color.white;

        foreach (var txt in reelTexts)
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

        if (!int.TryParse(inputBetAmount.text.Trim(), out int parse) || parse <= 0)
        {
            OnMessage(Color.red, "Invalid bet amount");
            return;
        }

        if (credits - parse >= 0)
        {
            credits -= parse;
            textCredits.text = $"Credits : {credits}";
            isStartSpin = true;

            pullButton.interactable = false;
        }
        else
        {
            OnMessage(Color.red, "You don't have enough money");
        }
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
        bool hasAnyMatch = false;

        // 이미지 초기화
        foreach (var img in reelImages)
            img.color = Color.white;

        //  잭팟 판별: 전체 3x5 슬롯이 모두 같은 값인지
        int firstValue = reelResults[0, 0];
        bool isJackpot = true;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (reelResults[row, col] != firstValue)
                {
                    isJackpot = false;
                    break;
                }
            }
            if (!isJackpot)
                break;
        }

        // 잭팟일 경우
        if (isJackpot)
        {
            textResult.text = " JACKPOT!!! ";
            credits += betAmount * 1000000;
            textCredits.text = $"Credits : {credits}";
            return;
        }

        // 일반 매치 확인
        bool verticalMatched = CheckVertical(betAmount);
        bool horizontalMatched = CheckHorizontal(betAmount);

        hasAnyMatch = verticalMatched || horizontalMatched;

        // 결과 표시
        textCredits.text = $"Credits : {credits}";
        textResult.text = hasAnyMatch ? "YOU WIN!!!" : "YOU LOSE!!!!";
    }


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

    private void OnMessage(Color color, string msg)
    {
        imageBetAmount.color = color;
        textResult.text = msg;
    }
    private bool CheckVertical(int betting)
    {
        bool isMatched = false;
        int matchCount = 0;

        for (int col = 0; col < 5; col++)
        {
            int a = reelResults[0, col];
            int b = reelResults[1, col];
            int c = reelResults[2, col];

            if (a == b && b == c)
            {
                isMatched = true;

                matchCount++;
                if (matchCount == 1)
                    credits += betting * 5;
                else if (matchCount >= 2)
                    credits += betting * 10;
               

                for (int row = 0; row < 3; row++)
                {
                    reelImages[row, col].color = customJackPot;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.15f));
                }
            }
        }

        return isMatched;
    }
    private bool CheckHorizontal(int betting)
    {

        bool isMatched = false;

        int matchCount = 0;
        for (int row = 0; row < 3; row++)  // 행 기준으로
        {
            int a = reelResults[row, 0];
            int b = reelResults[row, 1];
            int c = reelResults[row, 2];
            int d = reelResults[row, 3];
            int e = reelResults[row, 4];

            if (a == b && b == c && c == d && d == e)
            {

                isMatched = true;

                matchCount++;
                if (matchCount == 1)
                    credits += betting * 10;
                else if (matchCount >= 2)
                    credits += betting * 20;
               



                for (int col = 0; col < 5; col++)  // 열 순회
                {
                    reelImages[row, col].color = customJackPot;
                    StartCoroutine(BlinkText(reelTexts[row, col], 0.5f, 0.15f));
                }
            }

        }
        return isMatched;
    }


}
