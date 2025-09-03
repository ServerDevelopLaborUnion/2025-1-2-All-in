using TMPro;
using UnityEngine;

public class RandomMoney : ItemOn
{
    public override int probability { get; set; }
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI creditsText;

    public override void Itemon()
    {
        RandMoney();
    }
    private void RandMoney()
    {
        long money = Random.Range(4000, 15000);
        moneyManager.Money += money;
        creditsText.text = "Credits :" + moneyManager.Money;

    }
}
