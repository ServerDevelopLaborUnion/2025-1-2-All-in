using UnityEngine;
using UnityEngine.Rendering;

public class HorizontalPlus : ItemOn
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
        horizontalPlus();
    }
    public void horizontalPlus()
    {
        int final = probability + probabilityplus;
        if (Random.Range(1,100)<=final)
        {
            Debug.Log("Áõ°¡!");
            machine._horizontalChance += 0.05f;
        }
        else
        {
            Debug.Log("¾ÈµÊ"+ probability);

        }
    }
}
