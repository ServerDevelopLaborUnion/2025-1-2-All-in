using System.Collections;
using UnityEngine;

public class AutoSpinUntilJackpot : MonoBehaviour
{
    [SerializeField] private SloltMachine slotMachine; // ���� ���Ըӽ� ��ũ��Ʈ ����
    [SerializeField] private long betAmountOverride = 0; // 0�̸� ���Ըӽ��� �ּ� ���� ���

    private Coroutine autoSpinCoroutine;

    public void StartAutoSpin()
    {
        if (autoSpinCoroutine != null)
            StopCoroutine(autoSpinCoroutine);

        autoSpinCoroutine = StartCoroutine(AutoSpinLoop());
    }

    public void StopAutoSpin()
    {
        if (autoSpinCoroutine != null)
        {
            StopCoroutine(autoSpinCoroutine);
            autoSpinCoroutine = null;
        }
    }

    private IEnumerator AutoSpinLoop()
    {
        bool jackpotHit = false;

        while (!jackpotHit)
        {
            long bet = betAmountOverride > 0 ? betAmountOverride : slotMachine.GetMinimumBet();

            // ũ���� ���� �� ����
            if (slotMachine.GetCredits() < bet)
            {
                Debug.LogWarning("AutoSpin: Not enough credits.");
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
                Debug.Log("AutoSpin: JACKPOT HIT!");
            }

            // �ణ�� ��
            yield return new WaitForSeconds(0.2f);
        }

        autoSpinCoroutine = null;
    }
}
