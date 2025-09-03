using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField]private TextMeshProUGUI ticketText;
    [SerializeField]private SloltMachine machine;
    [SerializeField] private MoneyMangaer moneyManager;

    // 버튼 클릭 시 몇 개를 살지 매개변수로 받음
    public void BuyTicket(int amount , long cost)
    {
        if (moneyManager.Money >= cost)
        {
            Debug.Log("되어");
            moneyManager.Money -= cost;
            creditsText.text = "Credits :" + moneyManager.Money;
            machine.HaveSpin += amount;
            ticketText.text = "SPIN:" + machine.HaveSpin;

        }
        else
        {
            //돈이 부족합니다 panel 뛰우기
        }
    }
    public void Buy3()
    {
        BuyTicket(3, 300);
            machine.pullButton.interactable = true;
            machine.minBetButton.interactable = true;
            machine.maxBetButton.interactable = true;
    }
    public void Buy7()
    {

        BuyTicket(7, 600);
        machine.pullButton.interactable = true;
        machine.minBetButton.interactable = true;
        machine.maxBetButton.interactable = true;
    }

}
