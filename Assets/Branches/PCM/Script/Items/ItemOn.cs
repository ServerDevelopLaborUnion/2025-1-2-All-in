using System;
using System.Data;
using UnityEngine;

public abstract class ItemOn : MonoBehaviour
{
    public abstract int probability { get; set; } 
    private Action OnAbilityCast;
    public static int probabilityplus;
    private void Awake()
    {    
        
        OnAbilityCast += ItemsCheck;
    }
    private void Start()
    {
        probability += probabilityplus;
    }
    // 아이템이 가방에 들어있는지 확인
    public void ItemsCheck()
    {
            Itemon(); // 자식에서 오버라이드 가능        
    }

    
    public virtual void Itemon()
    {
     
    }
}

