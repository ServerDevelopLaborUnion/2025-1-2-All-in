using TMPro;
using UnityEngine;

public class RandomMoney : ItemOn
{
    public override int probability { get; set; }
    public override MoneyManager money { get; set; }

    private TextMeshProUGUI creditsText;
    private void Awake()
    {

        creditsText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
    }
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
        long a = Random.Range(4000, 15000);

        money.Money += a;
        Debug.Log(money.Money);
        creditsText.text = $"보유 금액 : {money.Money.ToString("N0")}원";

    }
}
