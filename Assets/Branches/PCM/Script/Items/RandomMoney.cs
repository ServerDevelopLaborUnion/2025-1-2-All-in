using TMPro;
using UnityEngine;

public class RandomMoney : ItemOn
{
    public override int probability { get; set; }
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI creditsText;

    public override void Itemon()
    {
        base.Itemon();
        if (gameObject != null)
        {
            RandMoney();
        }
    }
    private void RandMoney()
    {

        moneyManager.Money += Random.Range(4000, 15000);
        creditsText.text = "Credits :" + moneyManager.Money;

    }
}
