using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SlotRefund : ItemOn
{
    public override int probability { get; set; } = 100;
    public override MoneyManager money { get; set; }
     private SloltMachine machine;
    private MoneyManager moneyManager;
    private TextMeshProUGUI creditsText;

    private void Start()
    {
        machine = FindAnyObjectByType<SloltMachine>();
        creditsText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
        moneyManager = MoneyManager.Instance;
    }
    public override void Itemon()
    {
        base.Itemon();
        slotrefund();
    }
    private void slotrefund()
    {
        int final = probability += probabilityplus;
        long currentBet = machine.lastBetAmount;
        bool isJackpot = machine.CheckJackpot(currentBet); // 매개변수로 전달
        if (!isJackpot)
        {
            if (Random.Range(0, 100) <= final)
            {
                moneyManager.Money += (currentBet / 10);
                creditsText.text = $"보유 금액 : {money.Money.ToString("N0")}원";
                Debug.Log("야 된다!");
            }
        }
    }
}
