using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSpinUntilJackpot : MonoBehaviour
{
    [SerializeField] private SloltMachine slotMachine; // 기존 슬롯머신 스크립트 연결
    [SerializeField] private long betAmountOverride = 0; // 0이면 슬롯머신의 최소 배팅 사용
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

            // 크레딧 부족 시 종료
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
                _startbutton.interactable = true;
                Debug.Log("AutoSpin: JACKPOT HIT!");
            }

            // 약간의 텀
            yield return new WaitForSeconds(1f);
        }

        autoSpinCoroutine = null;
    }
}
