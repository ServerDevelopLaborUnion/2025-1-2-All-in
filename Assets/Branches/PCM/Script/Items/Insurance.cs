using TMPro;
using UnityEditor;
using UnityEngine;

public class Insurance : ItemOn
{
    [SerializeField]private MoneyManager moneymachine;   
    private TextMeshProUGUI ticketText;
    private TextMeshProUGUI creditText;
    [SerializeField]private SloltMachine machine;
    public override int probability { get; set; }
    public override MoneyManager money { get; set; }


    
    private bool deathcount = false;
    private void Awake()
    {
        ticketText = GameObject.Find(" Number of spins remaining").GetComponent<TextMeshProUGUI>();
        creditText = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (!deathcount &&moneymachine.Money == 0 && machine.HaveSpin ==0)
        {
            Debug.Log("двдв");

            insurance();
            deathcount = true;
        }
    }
    private void insurance()
    {        
        moneymachine.Money += 15000;
        machine.HaveSpin += 2;
        creditText.text = "Credit"+ moneymachine.Money;
        ticketText.text = "SPIN:" + machine.HaveSpin;
    }

}
