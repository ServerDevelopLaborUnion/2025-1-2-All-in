using System;
using UnityEngine;

public abstract class ItemOn : MonoBehaviour
{
    public abstract int probability { get; set; }
    private Action OnAbilityCast;

    private void Awake()
    {    
        OnAbilityCast += ItemsCheck;
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

