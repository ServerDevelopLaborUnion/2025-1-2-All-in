using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI ticketText;
    [SerializeField] private SloltMachine machine;
    [SerializeField] private AudioClip purchaseSound;
    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // 버튼 클릭 시 몇 개를 살지 매개변수로 받음

    public void BuyTicket(int amount, long cost)
    {
        Debug.Log($"진입{cost}, {MoneyManager.Instance.Money}");
        if (MoneyManager.Instance.Money >= cost)
        {
            Debug.Log("되어");
            MoneyManager.Instance.Money -= cost;
            creditsText.text = "Credits :" + MoneyManager.Instance.Money;
            machine.HaveSpin += amount;
            ticketText.text = "SPIN:" + machine.HaveSpin;

        }
        else
        {
            Debug.Log("돈이 부족함");
            //돈이 부족합니다 panel 뛰우기
        }
    }
    public void Buy3()
    {
        Debug.Log("e");
        BuyTicket(3, 300);
        machine.pullButton.interactable = true;
        machine.minBetButton.interactable = true;
        machine.maxBetButton.interactable = true;
        audio.PlayOneShot(purchaseSound);
    }
    public void Buy7()
    {

        BuyTicket(7, 1000);
        machine.pullButton.interactable = true;
        machine.minBetButton.interactable = true;
        machine.maxBetButton.interactable = true;
        audio.PlayOneShot(purchaseSound);
    }

}
