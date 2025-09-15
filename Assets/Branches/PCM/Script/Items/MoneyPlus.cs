using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoneyPlus : ItemOn
{
    public override int probability { get; set; } = 60;
    public override MoneyManager money { get; set; }
    private TextMeshProUGUI creditsText;
    [SerializeField]private int moneyplus;
    private void Awake()
    {       
        creditsText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Itemon();
        }
    }
    public override void Itemon()
    {
        base.Itemon();
        moneyPlus();
    }
    public void moneyPlus()
    {
        int final = probability + probabilityplus;
        if (Random.Range(0, 100) <= final)
        {
            money.Money += moneyplus;
            creditsText.text = $"보유 금액 : {money.Money.ToString("N0")}";
        }
    }
}
