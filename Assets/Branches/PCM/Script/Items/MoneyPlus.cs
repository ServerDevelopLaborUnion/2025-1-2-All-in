using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoneyPlus : ItemOn
{
    public override int probability { get; set; } = 60;
    [SerializeField]private SloltMachine machine;
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
            machine.Credits += moneyplus;
            creditsText.text = "Credits :" + machine.Credits;
        }
    }
}
