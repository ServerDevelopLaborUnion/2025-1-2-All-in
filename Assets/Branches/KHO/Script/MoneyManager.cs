using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private long _money = 0;
    private long _bestMoney;

    public long Money 
    { 
        get
        {
            return _money;
        }

        set
        {
            if (value > _money && value >= 10000)
            {
                BestMoney = value;
                _money = value;
            }
            else
            {
                _money = value;
            }
        }
    }

    public long BestMoney
    {
        get
        {
            return _bestMoney;
        }

        set
        {
            _bestMoney = value;
            MoneyGameData.Intance.UpdateDate();
        }
    }

    static public MoneyManager Instance { get; private set;}

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


    public void SetBset(long money)
    {
        _bestMoney = money;
    }
}
