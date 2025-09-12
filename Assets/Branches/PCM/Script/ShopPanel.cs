using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject dontHaveSpin;
    private RectTransform rect;
    public bool onActive { get; set; }
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
        if (onActive == true && Mouse.current.leftButton.wasPressedThisFrame)
        { 
            StartCoroutine(Wait());
            onActive = false;
        }
    }
    public void PanelDown()
    {
        if (machine.HaveSpin <= 0)
        {
            dontHaveSpin.transform.DOScale(new Vector3(2.7f, 0.26f, 0), 0.7f);
            onActive = true;
        }
    }
    public void PanelUp()
    {
        rect.DOAnchorPosY(500f, 2f).SetEase(Ease.OutQuint, 0.5f);
    }
    private IEnumerator Wait()
    {
        dontHaveSpin.transform.DOScale(new Vector3(2.7f, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.5f);
        rect.DOAnchorPosY(21f, 3f).SetEase(Ease.OutElastic, 0.5f);

    }
}


