using System;
using UnityEngine;

public class TargetAmountDown : ItemOn
{
    private DeadLine deadLine; 
    public override int probability { get; set; }

    public bool TargetDown()
    {
        if (gameObject.activeSelf)
        {
            Console.WriteLine("sdsagadgdasgasdfasfdasadfdasfadfads");
            return true;
        }
        else
        {
            return false;
        }
    }
}
