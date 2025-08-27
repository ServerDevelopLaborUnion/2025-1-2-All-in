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
        // Q Ű �Է��� ������ ���� üũ �� �ߵ�
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
        int final = probability + probabilityplus;
        if (Random.Range(1, 100) <= final) // 1~4 ���� ����!
        {
            ticketText.text = "SPIN:" + machine.HaveSpin;
            machine.HaveSpin += 2;
        }           
        else
        {
            //�˾Ƽ� �ٸ��� ���� �ֱ�
        }
    }
}
