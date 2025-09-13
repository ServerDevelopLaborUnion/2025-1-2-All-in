using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private ItemListSO _so;
    [SerializeField] private TextMeshProUGUI creditsText;
    private MoneyManager moneymahine;
    [SerializeField] private GameObject bag;
    [SerializeField] private Sprite _soldOut;
    [SerializeField]private SloltMachine machine; 

    // 모든 슬롯이 공유하는 전역 풀
    public static List<ItemsSO> drawItem = new List<ItemsSO>();

    private int _randitem = -1;
    private Image _skillimage;
    public GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemInformation;
    [SerializeField]private TextMeshProUGUI credits;
    private AudioSource audio;
    [SerializeField] private AudioClip reroll;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        _skillimage = GetComponent<Image>();
        infoPanel.SetActive(false);
    }

    private void Start()
    {
        moneymahine = MoneyManager.Instance;
        if (drawItem.Count == 0)
        {
            for (int i = 0; i < _so.List.Count; i++)
            {
                drawItem.Add(_so.List[i]);
            }
        }
        RandAllSlots();
    }

    public void OnClick()
    {
        RandAllSlots();
        audio.PlayOneShot(reroll);
        Debug.Log(moneymahine.Money.ToString("N0") + "됨");
        moneymahine.Money -= 1000;
        Debug.Log(moneymahine.Money.ToString("N0") + "됨");
        creditsText.text = "Credit:" + moneymahine.Money;

    }
    // 모든 슬롯이 동시에 랜덤 돌리는 함수
    public static void RandAllSlots()
    {
        Debug.Log("asdas");
        // 사용된 인덱스 초기화
        HashSet<int> usedIndexes = new HashSet<int>();


        RandomItem[] slots = Object.FindObjectsByType<RandomItem>(FindObjectsSortMode.None);
        foreach (var slot in slots)
        {
            slot.Rand(usedIndexes);
        }
    }


    private void Rand(HashSet<int> usedIndexes)
    {
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < drawItem.Count; i++)
        {
            if (drawItem[i] != null && !usedIndexes.Contains(i))
                availableIndexes.Add(i);
        }

        if (availableIndexes.Count == 0)
        {
            _randitem = -1;
            _skillimage.sprite = _soldOut;
            return;
        }

        int randIdx = availableIndexes[Random.Range(0, availableIndexes.Count)];
        _randitem = randIdx;
        usedIndexes.Add(_randitem);

        _skillimage.sprite = drawItem[_randitem].image;
        credits.text = "$" + drawItem[_randitem].money;
    }

    public void Buy()
    {
        if (_randitem >= 0 && _randitem < drawItem.Count)
        {
            ItemsSO data = drawItem[_randitem];
            moneymahine.Money -= data.money;
            creditsText.text = "Credits :" + moneymahine.Money;

            GameObject items = Instantiate(data.itemPrefab, bag.transform);
            items.SetActive(true);

            ItemOn itemOn = items.GetComponent<ItemOn>();
            if (itemOn != null)
            {
                machine.items.Add(itemOn);
            }

            drawItem[_randitem] = null; // 아이템 구매 처리
            audio.PlayOneShot(reroll);
        }

        _skillimage.sprite = _soldOut;
    }

    // 마우스가 버튼 위로 올라갈 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);

            if (_randitem >= 0 && _randitem < drawItem.Count && drawItem[_randitem] != null)
            {
                _itemName.text = drawItem[_randitem].itemName;
                _itemInformation.text = drawItem[_randitem].itemInformation;
            }
            else
            {
                _itemName.text = "";
                _itemInformation.text = "";
            }
        }
    }

    // 마우스가 버튼에서 나갈 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            _itemName.text = "";
            _itemInformation.text = "";
        }
    }
}
