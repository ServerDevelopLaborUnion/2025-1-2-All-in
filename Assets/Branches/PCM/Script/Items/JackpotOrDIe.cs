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
        if (Keyboard.current.fKey.wasPressedThisFrame)
        { 
            machine.jackpotChance = 0.25f;
            ver = machine._verticalChance;
            hor = machine._horizontalChance;
            machine._verticalChance = 0;
            machine._horizontalChance = 0;
            onAbility = true;
            textChance.text = $" 가로줄 : {machine._verticalChance * 100}% \n 세로줄 : {machine._horizontalChance * 100}% \n 잭팟 : {machine.jackpotChance * 100:F4}%";
            Debug.Log(onAbility);
        }
    }
    public void JackpotOrDieAction()
    {
        
        if (machine == null)
        {
            Debug.LogWarning("SloltMachine이 할당되지 않았습니다.");
            return;
        }

        // SloltMachine에서 lastBetAmount 가져오기
        long currentBet = machine.lastBetAmount;
        bool isJackpot = machine.CheckJackpot(currentBet); // 매개변수로 전달

        if (!isJackpot)
        {
            money.Money = 0;
            creditsText.text = $"보유 금액 : {money.Money.ToString("N0")}원";
            machine.jackpotChance = 0.00001f;
            machine._horizontalChance = hor;
            machine._verticalChance = ver;
            Debug.Log($"{hor} , {ver}");
        }
        else
        {
            machine.jackpotChance = 0.00001f;
            machine._horizontalChance = hor;
            machine._verticalChance = ver;
        }
            textChance.text = $" 가로줄 : {machine._verticalChance * 100}% \n 세로줄 : {machine._horizontalChance * 100}% \n 잭팟 : {machine.jackpotChance * 100:F4}%";

    }
}
