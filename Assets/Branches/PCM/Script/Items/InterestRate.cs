using UnityEngine;

public class InterestRate : ItemOn
{
    [SerializeField]private bool onActivated = false;

    public override int probability { get; set; }
    private void OnEnable()
    {
        onActivated = true;
    }
    public int Interest()
    {
       if (onActivated)
       {
        
            return 2;
       }
       else
       {
            return 0;
       }

    }
}
