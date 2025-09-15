using System;
using UnityEngine;

public class TargetAmountDown : ItemOn
{
    private DeadLine deadLine; 
    public override int probability { get; set; }
    public override MoneyManager money { get; set; }

    public bool TargetDown()
    {
        if (gameObject.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
