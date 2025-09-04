using UnityEngine;

public class NoBagDouble : ItemOn
{
    [SerializeField] private GameObject bag;
    public override int probability { get ; set; }

    public bool Nobagdouble()
    {
        if (bag.transform.childCount >= 1)
        {
            foreach (Transform child in bag.transform)
            {
                Destroy(child);
            }
        }
        return true;
    }
}
