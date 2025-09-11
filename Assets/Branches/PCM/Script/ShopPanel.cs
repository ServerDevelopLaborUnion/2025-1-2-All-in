using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.InputSystem;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject dontHaveSpin;
    private RectTransform rect;
    private bool onActive = false;
    private SloltMachine machine;

    private void Awake()
    {
        machine = FindAnyObjectByType<SloltMachine>();
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        dontHaveSpin.transform.localScale = Vector3.zero;
        //rect.DOAnchorPosY(-412.5f, 1).SetEase(Ease.OutExpo);// ¾Æ·¡·Î ´Ý±â
        //panelopen = false;
    }
    private void Update()
    {
        if (onActive == true&&Mouse.current.leftButton.wasPressedThisFrame) 
        {
            dontHaveSpin.transform.localScale = Vector3.zero;
            StartCoroutine(Wait());
            onActive = false;
        }
    }
    public void PanelDown()
    {
        if (machine.HaveSpin <= 0)
        {
            dontHaveSpin.transform.DOScale(new Vector3(1.1f,0.1f,0) , 0.7f);
            onActive = true;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        rect.DOAnchorPosY(21f, 3f).SetEase(Ease.OutElastic, 0.5f);
        
    }
}

