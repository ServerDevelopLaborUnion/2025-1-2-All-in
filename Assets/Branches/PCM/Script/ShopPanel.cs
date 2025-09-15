using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject dontHaveSpin;
    [SerializeField] private GameObject round1;
    private RectTransform rect;
    private bool roundActive;
    [field:SerializeField]public bool onActive { get; set; }
    private SloltMachine machine;
    private DeadLine deadLine;

    private bool isAnimating = false;

    private void Awake()
    {
        deadLine = FindAnyObjectByType<DeadLine>();
        machine = FindAnyObjectByType<SloltMachine>();
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        dontHaveSpin.transform .localScale = Vector3.zero;
        round1.transform.localScale = Vector3.zero;
    }
    private void Update()
    {
        // 애니메이션 중이면 입력 무시
        if (isAnimating) return;

        if (onActive == true && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (deadLine._compensation == 1)
            {
                StartCoroutine(ShowRound1());
            }
            StartCoroutine(Wait());
            onActive = false;
        }

        if (roundActive == true && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(HideRound1());
        }
    }

    private IEnumerator ShowRound1()
    {
        isAnimating = true;
        round1.transform.DOKill();
        yield return round1.transform
            .DOScale(new Vector3(1, 1, 0), 0.7f)
            .WaitForCompletion();
        isAnimating = false;
    }

    private IEnumerator HideRound1()
    {
        isAnimating = true;
        round1.transform.DOKill();
        yield return round1.transform
            .DOScale(new Vector3(1, 0, 0), 0.3f)
            .WaitForCompletion();
        isAnimating = false;
    }
    public void PanelDown()
    {
        if (machine.HaveSpin <= 0)
        {
            dontHaveSpin.transform.DOScale(new Vector3(1, 1, 0), 0.7f);
            deadLine._compensation -= 1;
            onActive = true;
        }
    }
    public void PanelUp()
    {
        if (deadLine.Oninterest == true)
        {
            rect.DOAnchorPosY(1200f, 2f).SetEase(Ease.OutQuint, 0.5f);
        }
    }
    private IEnumerator Wait()
    {
        isAnimating = true;

        dontHaveSpin.transform.DOKill();
        round1.transform.DOKill();
        rect.DOKill();

        dontHaveSpin.transform.DOScale(new Vector3(1, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.5f);

        round1.transform.DOScale(new Vector3(1, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.5f);

        yield return rect.DOAnchorPosY(21f, 3f)
            .SetEase(Ease.OutElastic, 0.5f)
            .WaitForCompletion();

        isAnimating = false;
    }
}


