using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JackpotOrDie : ItemOn
{
    public override int probability { get; set; }
    [SerializeField] private TextMeshProUGUI textChance;
    private SloltMachine machine;
    public override MoneyManager money { get; set; }
    private TextMeshProUGUI creditsText;
    public bool onAbility { get; set; } = false;
    private float ver;
    private float hor;
    private void Awake()
    {
        machine = FindAnyObjectByType<SloltMachine>();    
        creditsText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        textChance  = GameObject.Find("JackpotProbabilityText").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame&& !onAbility)
        { 
            machine.jackpotChance = 0.10f;
            ver = machine._verticalChance;
            hor = machine._horizontalChance;
            machine._verticalChance = 0.01f;
            machine._horizontalChance = 0.01f;
            onAbility = true;
            textChance.text = $" ∞°∑Œ¡Ÿ : {machine._verticalChance * 100}% \n ºº∑Œ¡Ÿ : {machine._horizontalChance * 100}% \n ¿Ë∆Ã : {machine.jackpotChance * 100:F4}%";
            onAbility = true;
        }
    }
    public override void Itemon()
    {
        base.Itemon();
    }
  
}
