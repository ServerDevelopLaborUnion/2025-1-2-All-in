using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemsSO itemSO;

    private GameObject _panel;
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _itemInformation;

    private string _defaultName;
    private string _defaultInfo;

    private void Start()
    {
        _panel = GameObject.Find("Information");

        if (_panel != null)
        {
            _itemName = _panel.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            _itemInformation = _panel.transform.Find("ItemInformation").GetComponent<TextMeshProUGUI>();

            _defaultName = _itemName.text;
            _defaultInfo = _itemInformation.text;

            _panel.SetActive(false); // 시작 시 비활성화
        }
        else
        {
            Debug.LogError("Information 패널을 찾을 수 없음");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null && _panel != null)
        {
            _panel.SetActive(true);
            _itemName.text = itemSO.itemName;
            _itemInformation.text = itemSO.itemInformation;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_panel != null)
        {
            _panel.SetActive(false);
            _itemName.text = _defaultName;
            _itemInformation.text = _defaultInfo;
        }
    }
}
