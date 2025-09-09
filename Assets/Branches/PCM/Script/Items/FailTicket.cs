using TMPro;
using UnityEngine;

public class FailTicket : ItemOn
{
    [SerializeField]private TextMeshProUGUI ticketText;
    public override int probability { get; set; } = 50;
    private SloltMachine machine;
    

    private void Awake()
    {
        machine = GameObject.FindAnyObjectByType<SloltMachine>();
    }
    public override void Itemon()
    {
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
