using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SloltMachine2 : MonoBehaviour
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
        int random_spinResult1 = Random.Range(0, 2);
        int random_spinResult2 = Random.Range(0, 2);
        int random_spinResult3 = Random.Range(0, 2);

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

        foreach (var img in reelImages)
            img.color = Color.white;

        Color32 customTwo = new Color32(178, 34, 34, 255);
        Color32 customJackPot = new Color32(255, 239, 184, 255);

        bool isJackpot = true;
        int firstValue = reelResults[0, 0];

        for (int row = 0; row < 3 && isJackpot; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (reelResults[row, col] != firstValue)
                {
                    isJackpot = false;
                    break;
                }
            }
        }

        int matchCount = 0;
        for (int col = 0; col < 5; col++)
        {
            int a = reelResults[0, col];
            int b = reelResults[1, col];
            int c = reelResults[2, col];

            if (a == b && b == c)
            {
                matchCount++;
                if (matchCount == 1)
                    credits += betAmount * 2;
                else if (matchCount == 2)
                    credits += betAmount * 3;
                else if (matchCount >= 3)
                    credits += betAmount * 4;

                for (int row = 0; row < 3; row++)
                {
                    reelImages[row, col].color = customJackPot;
                    StartCoroutine(BlinkText(reelTexts[row, col], 1.5f, 0.15f));
                }
            }
            else if (f1ReelResult == f2ReelResult ||
                     f1ReelResult == f3ReelResult ||
                     f2ReelResult == f3ReelResult)
            {
                if (f1ReelResult == f2ReelResult)
                {
                    f1ReelImage.color = customTwo;
                    s2ReelImage.color = customTwo;
                }
                if (f1ReelResult == f3ReelResult)
                {
                    f1ReelImage.color = customTwo;
                    t3ReelImage.color = customTwo;
                }
                if (f2ReelResult == f3ReelResult)
                {
                    s2ReelImage.color = customTwo;
                    t3ReelImage.color = customTwo;
                }

                credits += betAmount / 2;
                textCredits.text = $"Credits : {credits}";
                textResult.text = "Matched Two!";
            }
            else
            {
                textResult.text = "YOU LOSE!!!!";
            }
        }
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
}
