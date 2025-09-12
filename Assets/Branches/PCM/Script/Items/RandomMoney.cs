using TMPro;
using UnityEngine;

public class RandomMoney : ItemOn
{
    public override int probability { get; set; }
    public override MoneyManager money { get; set; }

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
        long a = Random.Range(4000, 15000);

        money.Money += a;
        Debug.Log(money.Money);
        creditsText.text = "Credits :" + money.Money;

    }
}
