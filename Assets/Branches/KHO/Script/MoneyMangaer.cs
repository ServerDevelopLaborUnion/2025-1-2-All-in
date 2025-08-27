using UnityEngine;

public class MoneyMangaer : MonoBehaviour
{
    private long _money = 0;

    public long Money 
    { 
        get
        {
            return _money;
        }

        set
        {
            _money = value;
            MoneyGameData.Intance.UpdateDate();
        }
    }

    static public MoneyMangaer Instance { get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
