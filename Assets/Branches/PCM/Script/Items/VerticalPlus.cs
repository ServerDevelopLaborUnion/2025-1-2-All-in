using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalPlus : ItemOn
{
    public override int probability { get; set; } = 10;
    public override MoneyManager money { get; set; }
     private SloltMachine machine;
    private void Awake()
    {
        machine = FindAnyObjectByType<SloltMachine>();        
    }
    private void Update()
    {
        
    }
    public override void Itemon()
    {
        base.Itemon();
        verticalPlus();
    }
    private void verticalPlus()
    {
        int final = probability + probabilityplus;
        if(Random.Range(1,100)<= final)
        {
            machine._verticalChance += 0.05f;
        }
    }
}
