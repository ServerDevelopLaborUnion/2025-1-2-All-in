using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField]private TextMeshProUGUI ticketText;
    [SerializeField]private SloltMachine machine;
    [SerializeField] private MoneyMangaer moneyManager;

    // ��ư Ŭ�� �� �� ���� ���� �Ű������� ����
    public void BuyTicket(int amount , long cost)
    {
        if (moneyManager.Money >= cost)
        {
            Debug.Log("�Ǿ�");
            moneyManager.Money -= cost;
            creditsText.text = "Credits :" + moneyManager.Money;
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
