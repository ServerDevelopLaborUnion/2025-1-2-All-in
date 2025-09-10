using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Constraints;

public class ShopPanel : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private bool panelopen = false; // 현재 열렸는지 여부
    [SerializeField] private DeadLine _deadLine;
    [SerializeField] private SloltMachine _machine;
    [SerializeField] GameObject _deadLineText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        rect.DOAnchorPosY(-412.5f, 1).SetEase(Ease.OutExpo);// 아래로 닫기
        _deadLineText.SetActive(false);
        panelopen = false;
    }
    private void FixedUpdate()
    {
        if (_machine.GetCredits() <= 0 && _machine._panel == true)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (_machine.HaveSpin <= 0)
        {
            if (!panelopen)
            {
                rect.DOAnchorPosY(-56f, 1).SetEase(Ease.OutExpo);// 위로 열기
                _deadLine.MoneyP();
                _deadLineText.SetActive(true);
                panelopen = true;
            }
        }
        else if (panelopen)
        {
            _deadLineText.SetActive(false);
            rect.DOAnchorPosY(-412.5f, 1).SetEase(Ease.OutExpo);// 아래로 닫기
            panelopen = false;
        }
    }
}

