using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SlotRefund : ItemOn
{
    public override int probability { get; set; } = 100;
    [SerializeField]private SloltMachine machine;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI creditsText;

    public override void Itemon()
    {
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
