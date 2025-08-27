using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public List<GameObject> bag = new List<GameObject>();

    // �ܺο��� ������Ʈ �ְ� ���� �� ȣ���� �Լ�
    public void AddItem(GameObject item, GameObject prefab)
    {
        // �̹� ���� prefab ��� �������� �ִ��� Ȯ��
        if (bag.Any(x => x.name.Contains(prefab.name)))
        {
            Debug.Log($"{prefab.name} �̹� ���濡 ����");
        }
        else
        {
            bag.Add(item);
            Debug.Log($"{item.name} �������� ���濡 ��!");
        }
    }


    // ������Ʈ ����
    public void RemoveItem(GameObject item)
    {
        if (bag.Contains(item))
        {
            bag.Remove(item);
            Debug.Log($"{item.name} �������� ���濡�� ���ŵ�!");
        }
    }
}
