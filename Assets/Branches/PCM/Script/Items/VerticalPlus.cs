using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalPlus : ItemOn
{
    public override int probability { get; set; } = 10;
    [SerializeField] private SloltMachine machine;
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Itemon();
        }
    }
    public override void Itemon()
    {
        verticalPlus();
    }
    private void verticalPlus()
    {
        int final = probability + probabilityplus;
        if(Random.Range(1,100)<= final)
        {
            Debug.Log(final);
            machine.VerticalChance += 0.5f;
        }
    }
}
