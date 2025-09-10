using UnityEngine;
using UnityEngine.InputSystem;

public class ProbabilityPlus : ItemOn
{
    public override int probability { get; set;}

    private void Update()
    {
        
    }
    public override void Itemon()
    {
        base.Itemon();
        probabilityPlus();
    }
    private void probabilityPlus()
    {
        if(Random.Range(1, 100) <= 20)
        {
            probabilityplus += 5;
            Debug.Log(probabilityplus);
        }
    }
}
