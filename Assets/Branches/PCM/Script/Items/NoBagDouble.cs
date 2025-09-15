using UnityEngine;

public class NoBagDouble : ItemOn
{
    [SerializeField] private GameObject bag;
    public override int probability { get; set; }
    public override MoneyManager money { get; set; }

    public bool Nobagdouble()
    {
        if (bag.transform.childCount >= 1)
        {
            Destroy(gameObject);
        }
        return true;
    }
}
