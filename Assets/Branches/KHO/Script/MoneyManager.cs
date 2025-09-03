using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private long _money = 0;
    private long _bestMoney;
    public long Money
    {
        get => _money;
        set
        {
            long max = long.MaxValue;

            if (value < 0)
                _money = 0;
            else if (value > max)
                _money = max;
            else
                _money = value;

            if (_money > _bestMoney && _money >= 100000)
               BestMoney = _money;
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

    private void Start()
    {
        MoneyGameData.Intance.GetData(ref _bestMoney);
    }



    public void SetBset(long money)
    {
        _bestMoney = money;
    }
    private void Update()
    {
        Debug.Log(_bestMoney);
    }
}
