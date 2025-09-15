using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JackpotOrDie : ItemOn
{
    public override int probability { get; set; }
    [SerializeField] private SloltMachine machine;
    public override MoneyManager money { get; set; }
    private TextMeshProUGUI creditsText;
    public bool onAbility { get; set; } = false;
    private void Awake()
    {
        
        creditsText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            machine.jackpotChance = 0.25f;
            onAbility = true;
        }
    }
    public void JackpotOrDieAction()
    {
        if (machine == null)
        {
            Debug.LogWarning("SloltMachine이 할당되지 않았습니다.");
            return;
        }

        // SloltMachine에서 lastBetAmount 가져오기
        long currentBet = machine.lastBetAmount;
        bool isJackpot = machine.CheckJackpot(currentBet); // 매개변수로 전달

        if (!isJackpot)
        {
            money.Money = 0;
            creditsText.text = $"보유 금액 : {money.Money.ToString("N0")}원";
            machine.jackpotChance = 0.00001f;
        }
        else
        {
            machine.jackpotChance = 0.00001f;
        }

    }
}
