using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicketPlus : ItemOn
{
    private SloltMachine machine;
    [SerializeField] private TextMeshProUGUI ticketText;

    public override int probability { get; set; } = 40;

    private void Awake()
    {
        machine = FindAnyObjectByType<SloltMachine>();
    }

    private void Update()
    {
        // Q 키 입력이 들어오면 가방 체크 후 발동
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ItemsCheck();
        }
    }

    public override void Itemon()
    {
        TicketsPlus();
    }

    private void TicketsPlus()
    {
        
        if (Random.Range(1, 100) <= probability ) // 1~4 범위 주의!
        {
            ticketText.text = "SPIN:" + machine.HaveSpin;
            machine.HaveSpin += 2;
        }           
        else
        {
            //알아서 다른거 집어 넣기
        }
    }
}
