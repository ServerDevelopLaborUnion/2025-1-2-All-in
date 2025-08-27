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
            rect.DOAnchorPosY(-56f, 1).SetEase(Ease.OutExpo);// ���� ����
            panelopen = true; 
        }
        else if(panelopen)
        {
            rect.DOAnchorPosY(-412.5f, 1).SetEase(Ease.OutExpo);// �Ʒ��� �ݱ�
            panelopen = false;
        }
    }
}

