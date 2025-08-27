using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField]private TextMeshProUGUI ticketText;
    [SerializeField]private SloltMachine machine;

    // 버튼 클릭 시 몇 개를 살지 매개변수로 받음
    public void BuyTicket(int amount , long cost)
    {
        if (machine.Credits >= cost)
        {
            machine.Credits -= cost;
            creditsText.text = "Credits :" + machine.Credits;
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
    }
    public void Buy7()
    {
        BuyTicket(7, 600);
    }

}
