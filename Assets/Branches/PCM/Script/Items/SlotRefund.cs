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
        bool isJackpot = machine.CheckJackpot(currentBet); // �Ű������� ����
        if (!isJackpot)
        {
            if (Random.Range(0, 100) <= final)
            {
                moneyManager.Money += (currentBet / 10);
                creditsText.text = "Credit" + moneyManager.Money;
                Debug.Log("�� �ȴ�!");
            }
        }
    }
}
