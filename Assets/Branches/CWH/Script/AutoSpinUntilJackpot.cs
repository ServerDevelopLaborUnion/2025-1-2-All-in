using System.Collections;
using UnityEngine;

public class AutoSpinUntilJackpot : MonoBehaviour
{
    [SerializeField] private SloltMachine slotMachine; // 기존 슬롯머신 스크립트 연결
    [SerializeField] private long betAmountOverride = 0; // 0이면 슬롯머신의 최소 배팅 사용

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

            // 크레딧 부족 시 종료
            if (slotMachine.GetCredits() < bet)
            {
                Debug.LogWarning("AutoSpin: Not enough credits.");
                break;
            }

            // 슬롯머신에 배팅금액 설정
            slotMachine.SetBetAmount(bet);

            // 스핀 시작
            slotMachine.OnClickpull();

            // 스핀이 끝날 때까지 대기
            yield return new WaitUntil(() => !slotMachine.IsSpinning());

            // 잭팟 여부 확인
            if (slotMachine.IsJackpotHit(bet))
            {
                jackpotHit = true;
                Debug.Log("AutoSpin: JACKPOT HIT!");
            }

            // 약간의 텀
            yield return new WaitForSeconds(0.2f);
        }

        autoSpinCoroutine = null;
    }
}
