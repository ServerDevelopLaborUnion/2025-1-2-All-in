using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicketPlus : ItemOn
{
    private SloltMachine machine;
    [SerializeField] private TextMeshProUGUI ticketText;

    public override int probability { get; set; } = 40;
    public override MoneyManager money { get; set; }

    private void Awake()
    {
        machine = FindAnyObjectByType<SloltMachine>();
    }

    private void Update()
    {
        
        // Q 키 입력이 들어오면 가방 체크 후 발동
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Itemon();            
        }
    }

    public override void Itemon()
    {
        base.Itemon();
        TicketsPlus();
    }

    private void TicketsPlus()
    {
        int final = probability + probabilityplus;
        if (Random.Range(1, 100) <= final)
        {
            ticketText.text = ""+machine.HaveSpin;
            Debug.Log($"된다 , {machine.HaveSpin}");
            machine.HaveSpin += 1;
        }           
    }
}
