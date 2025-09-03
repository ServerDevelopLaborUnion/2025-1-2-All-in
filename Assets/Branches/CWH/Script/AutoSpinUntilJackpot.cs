using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSpinUntilJackpot : MonoBehaviour
{
    [SerializeField] private SloltMachine slotMachine; // ���� ���Ըӽ� ��ũ��Ʈ ����
    [SerializeField] private long betAmountOverride = 0; // 0�̸� ���Ըӽ��� �ּ� ���� ���
    [SerializeField] private TMP_InputField autoBetAmount;
    [SerializeField] private Button _startbutton;

    private Coroutine autoSpinCoroutine;

    public void StartAutoSpin()
    {
        if (autoSpinCoroutine != null)
            StopCoroutine(autoSpinCoroutine);

        _startbutton.interactable = false;
        autoSpinCoroutine = StartCoroutine(AutoSpinLoop());
    }

    public void StopAutoSpin()
    {
        if (autoSpinCoroutine != null)
        {
            _startbutton.interactable = true;
            StopCoroutine(autoSpinCoroutine);
            autoSpinCoroutine = null;
        }
    }

    private IEnumerator AutoSpinLoop()
    {
        string input = autoBetAmount.text.Trim().Replace(",", "");
        bool success = long.TryParse(input, out long autobet);
        bool jackpotHit = false;
        if (autoBetAmount == null)
            betAmountOverride = 0;
        else
            betAmountOverride = autobet;


        while (!jackpotHit)
        {
            long bet = betAmountOverride > 0 ? betAmountOverride : slotMachine.GetMinimumBet();

            // ũ���� ���� �� ����
            if (slotMachine.GetCredits() < bet)
            {
                Debug.LogWarning("AutoSpin: Not enough credits.");
                break;
            }
            if (slotMachine.HaveSpin <= 0)
            {
                Debug.LogWarning("AutoSpin: Not enough spins.");
                break;
            }
            // ���Ըӽſ� ���ñݾ� ����
            slotMachine.SetBetAmount(bet);

            // ���� ����
            slotMachine.OnClickpull();

            // ������ ���� ������ ���
            yield return new WaitUntil(() => !slotMachine.IsSpinning());

            // ���� ���� Ȯ��
            if (slotMachine.IsJackpotHit(bet))
            {
                jackpotHit = true;
                _startbutton.interactable = true;
                Debug.Log("AutoSpin: JACKPOT HIT!");
            }

            // �ణ�� ��
            yield return new WaitForSeconds(1f);
        }

        autoSpinCoroutine = null;
    }
}
