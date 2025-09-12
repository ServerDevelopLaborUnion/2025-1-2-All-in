using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SlotRefund : ItemOn
{
    public override int probability { get; set; } = 100;
    public override MoneyManager money { get; set; }
    [SerializeField] private SloltMachine machine;
    private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI creditsText;

    private void Start()
    {
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
                creditsText.text = "Credit" + moneyManager.Money;
                Debug.Log("야 된다!");
            }
        }
    }
}
