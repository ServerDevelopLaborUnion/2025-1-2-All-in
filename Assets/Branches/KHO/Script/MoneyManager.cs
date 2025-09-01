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
            if (value > _money)
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
            if (_money > _bestMoney)
            {
                _bestMoney = value;
            }
            else
            {
                return;
            }
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
