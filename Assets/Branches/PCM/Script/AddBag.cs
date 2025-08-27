using UnityEngine;
using UnityEngine.InputSystem;

public class AddBag : MonoBehaviour
{
   private Bag bag;
    [SerializeField] private GameObject items;
    private void Awake()
    {
        bag = FindAnyObjectByType<Bag>();
    }
    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            GameObject news = Instantiate(items);
            bag.AddItem(news ,items);
        }
        //���� id üũ �������� �ٲٱ�

    }
}
