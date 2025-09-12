using System;
using System.Data;
using UnityEngine;

public abstract class ItemOn : MonoBehaviour
{
    public abstract int probability { get; set; }
    public Action OnAbilityCast;
    public int probabilityplus;
    public abstract MoneyManager money { get; set; }

    private void Start()
    {
        money = MoneyManager.Instance;
        OnAbilityCast += Itemon;
        SloltMachine slolt = FindAnyObjectByType<SloltMachine>();
    }
    // 아이템이 가방에 들어있는지 확인    
    public virtual void Itemon()
    {
        Debug.Log("된다");
    }
}

