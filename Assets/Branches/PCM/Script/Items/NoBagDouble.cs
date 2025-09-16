using UnityEngine;

public class NoBagDouble : ItemOn
{
    [SerializeField] private GameObject bag;
    public override int probability { get; set; } = 30;
    public override MoneyManager money { get; set; }

    public bool Nobagdouble()
    {
        int final = probability + probabilityplus;
        if (Random.Range(0, 100 )< final)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
