using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour
{
    [SerializeField] private ItemListSO _so;
    [SerializeField] private MoneyManager moneymahine;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private GameObject bag;
    [SerializeField] private Sprite _soldOut;

    // 모든 슬롯이 공유하는 전역 풀
    public static List<ItemsSO> drawItem = new List<ItemsSO>();

    private int _randitem = -1;
    private Image _skillimage;

    private void Awake()
    {
        _skillimage = GetComponent<Image>();
    }

    private void Start()
    {
        if (drawItem.Count == 0)
        {
            for (int i = 0; i < _so.List.Count; i++)
            {
                drawItem.Add(_so.List[i]);
            }
        }

        RandAllSlots();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            RandAllSlots();
        }
    }

    // 모든 슬롯이 동시에 랜덤 돌리는 함수
    public static void RandAllSlots()
    {
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

            drawItem[_randitem] = null; // 아이템 구매 처리
        }

        _skillimage.sprite = _soldOut;
    }
}
