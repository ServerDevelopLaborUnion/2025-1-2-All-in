using UnityEngine;
using UnityEngine.InputSystem;

public class ProbabilityPlus : ItemOn
{
    public override int probability { get; set;}

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Itemon();
        }
    }
    public override void Itemon()
    {
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
