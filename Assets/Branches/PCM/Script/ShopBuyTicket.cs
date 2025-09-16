using TMPro;
using UnityEngine;

public class ShopBuyTicket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI ticketText;
    [SerializeField] private TextMeshProUGUI text7;
    [SerializeField] private TextMeshProUGUI text3;
    [SerializeField] private SloltMachine machine;
    [SerializeField] private AudioClip purchaseSound;
    private AudioSource audio;
    public bool _3onActive { get; set; } = false;
    public bool _7onActive { get; set; } = false;

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
            creditsText.text = $"보유 금액 : {MoneyManager.Instance.Money.ToString("N0")}";
            machine.HaveSpin += amount;
            ticketText.text = $"{machine.HaveSpin}";

        }
        else
        {
            Debug.Log("돈이 부족함");
            //돈이 부족합니다 panel 뛰우기
        }
    }
    public void Buy3()
    {
        if (!_3onActive)
        {
            BuyTicket(10, 3000);
            machine.pullButton.interactable = true;
            machine.minBetButton.interactable = true;
            machine.maxBetButton.interactable = true;
            audio.PlayOneShot(purchaseSound);
            text3.text = "품절";
            text3.color = Color.red;
        }
        _3onActive = true;
    }
    public void Buy7()
    {
        if (!_7onActive)
        {
            BuyTicket(20, 7000);
            machine.pullButton.interactable = true;
            machine.minBetButton.interactable = true;
            machine.maxBetButton.interactable = true;
            audio.PlayOneShot(purchaseSound);
            text7.text = "품절";
            text7.color = Color.red;
        }
        _7onActive = true;
    }

}
