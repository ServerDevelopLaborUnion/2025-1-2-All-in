using TMPro;
using UnityEditor;
using UnityEngine;

public class Insurance : ItemOn
{
    [SerializeField]private MoneyMangaer moneymachine;   
    [SerializeField]private TextMeshProUGUI ticketText;
    [SerializeField] private TextMeshProUGUI creditText;
    [SerializeField]private SloltMachine machine;
    public override int probability { get; set; }
    private bool deathcount = false;
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
