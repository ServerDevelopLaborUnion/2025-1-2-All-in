using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public List<GameObject> bag = new List<GameObject>();

    // 외부에서 오브젝트 넣고 싶을 때 호출할 함수
    public void AddItem(GameObject item, GameObject prefab)
    {
        // 이미 같은 prefab 출신 아이템이 있는지 확인
        if (bag.Any(x => x.name.Contains(prefab.name)))
        {
            Debug.Log($"{prefab.name} 이미 가방에 있음");
        }
        else
        {
            bag.Add(item);
            Debug.Log($"{item.name} 아이템이 가방에 들어감!");
        }
    }


    // 오브젝트 제거
    public void RemoveItem(GameObject item)
    {
        if (bag.Contains(item))
        {
            bag.Remove(item);
            Debug.Log($"{item.name} 아이템이 가방에서 제거됨!");
        }
    }
}
