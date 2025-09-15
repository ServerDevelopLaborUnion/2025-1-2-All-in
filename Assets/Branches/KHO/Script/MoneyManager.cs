using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MoneyManager : MonoBehaviour
{
    private long _money = 0;
    private long _bestMoney;
    private int _rankmin = 150000;
    public bool Dead { get; private set; } = false;
    public long Money
    {
        get => _money;
        set
        {
            long max = long.MaxValue;

            if (_money == 0 && value <= 0)
                Dead = true;

            if (value < 0)
                _money = 0;
            else if (value > max)
                _money = max;
            else
                _money = value;

            if (_money > _bestMoney && _money >= _rankmin)
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

        //_bestMoney = GetComponent<BackEndManager>().best;
    }





    public void SetBset(long money)
    {
        _bestMoney = money;
    }
    private void Update()
    {
        //Debug.Log(_bestMoney);
    }


    public void Setbeest(long best)
    {
        _bestMoney = best;
    }
}
