using UnityEngine;
using UnityEngine.Rendering;

public class HorizontalPlus : ItemOn
{
    public override int probability { get; set; } = 100;
    [SerializeField] private SloltMachine machine;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            Itemon();
        }
    }
    public override void Itemon()
    {
        horizontalPlus();
    }
    public void horizontalPlus()
    {
        int final = probability + probabilityplus;
        if (Random.Range(1,100)<=final)
        {
            Debug.Log("Áõ°¡!");
            machine._horizontalChance += 0.5f;
        }
        else
        {
            Debug.Log("¾ÈµÊ"+ probability);

        }
    }
}
