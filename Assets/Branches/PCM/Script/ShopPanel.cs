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
    public bool onActive { get; set; }
    private SloltMachine machine;
    private DeadLine deadLine;

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
        if (onActive == true && Mouse.current.leftButton.wasPressedThisFrame)
        { 
            if (deadLine._rounds == 1)
            {
                round1.transform.DOScale(new Vector3(2.7f, 0.26f, 0), 0.7f);
            }
            StartCoroutine(Wait());
            onActive = false;
        }
        
        if (roundActive == true&& Mouse.current.leftButton.wasPressedThisFrame)
        {
            round1.transform.DOScale(new Vector3(2.7f, 0, 0), 0.3f);
            roundActive = false;
        }
    }
    public void PanelDown()
    {
        if (machine.HaveSpin <= 0)
        {
            dontHaveSpin.transform.DOScale(new Vector3(2.7f, 0.26f, 0), 0.7f);
            deadLine._rounds -= 1;
            onActive = true;
        }
    }
    public void PanelUp()
    {
        if (deadLine.Oninterest == true)
        {
            rect.DOAnchorPosY(500f, 2f).SetEase(Ease.OutQuint, 0.5f);
        }
    }
    private IEnumerator Wait()
    {
        dontHaveSpin.transform.DOScale(new Vector3(2.7f, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.5f);
        round1.transform.DOScale(new Vector3(2.7f, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.5f);
        rect.DOAnchorPosY(21f, 3f).SetEase(Ease.OutElastic, 0.5f);

    }
}


