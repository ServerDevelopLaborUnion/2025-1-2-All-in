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
    // �������� ���濡 ����ִ��� Ȯ��
    public void ItemsCheck()
    {
            Itemon(); // �ڽĿ��� �������̵� ����        
    }

    
    public virtual void Itemon()
    {
     
    }
}

