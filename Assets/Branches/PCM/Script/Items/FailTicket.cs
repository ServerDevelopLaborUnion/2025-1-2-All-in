using TMPro;
using UnityEngine;

public class FailTicket : ItemOn
{
    private TextMeshProUGUI ticketText;
    public override int probability { get; set; } = 50;
    public override MoneyManager money { get; set; }
    private SloltMachine machine;

    private void Start()
    {
        ticketText = GameObject.Find(" Number of spins remaining").GetComponent<TextMeshProUGUI>();
        
    }
    private void Awake()
    {
        machine = GameObject.FindAnyObjectByType<SloltMachine>();
    }
    public override void Itemon()
    {
        base.Itemon();
        Failticket();
    }
    private void Failticket()
    {
         Debug.Log("µÈ´Ù");
        if (Random.Range(0, 100) < probability&& !machine.hasMatch)
        {
            machine.HaveSpin += 2;
            ticketText.text = machine.HaveSpin.ToString();
            
        }
    }
}
