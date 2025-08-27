using UnityEngine;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private bool panelopen = false; // ���� ���ȴ��� ����

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnClick()
    {
       

        if (!panelopen)
        {
            rect.DOAnchorPosY(-38, 1).SetEase(Ease.OutExpo);// ���� ����
            panelopen = true; 
        }
        else if(panelopen)
        {
            rect.DOAnchorPosY(-300, 1).SetEase(Ease.OutExpo);// �Ʒ��� �ݱ�
            panelopen = false;
        }
    }
}

