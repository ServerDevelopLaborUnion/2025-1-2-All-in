using UnityEngine;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private bool panelopen = false; // 현재 열렸는지 여부

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnClick()
    {
       

        if (!panelopen)
        {
            rect.DOAnchorPosY(-56f, 1).SetEase(Ease.OutExpo);// 위로 열기
            panelopen = true; 
        }
        else if(panelopen)
        {
            rect.DOAnchorPosY(-412.5f, 1).SetEase(Ease.OutExpo);// 아래로 닫기
            panelopen = false;
        }
    }
}

