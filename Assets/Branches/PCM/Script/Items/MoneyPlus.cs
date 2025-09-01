using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoneyPlus : ItemOn
{
    public override int probability { get; set; } = 60;
    [SerializeField]private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField]private int moneyplus;

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Itemon();
        }
    }
    public override void Itemon()
    {
        moneyPlus();
    }
    public void moneyPlus()
    {
        int final = probability + probabilityplus;
        if (Random.Range(0, 100) <= final)
        {
            moneyManager.Money += moneyplus;
            creditsText.text = "Credits :" + moneyManager.Money;
        }
    }
}
