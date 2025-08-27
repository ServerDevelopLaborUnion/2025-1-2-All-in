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
        if(Random.Range(1,100)<= probability)
        {
            machine._verticalChance += 0.5f;
        }
    }
}
