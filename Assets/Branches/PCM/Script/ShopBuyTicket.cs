using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField]private TextMeshProUGUI ticketText;
    [SerializeField]private SloltMachine machine;

    // ��ư Ŭ�� �� �� ���� ���� �Ű������� ����
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
            //���� �����մϴ� panel �ٿ��
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
