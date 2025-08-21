using UnityEngine;

[CreateAssetMenu(fileName = "OreSO", menuName = "Scriptable Objects/OreSO")]
public class OreSO : ScriptableObject
{
    public string oreName;
    public Sprite OreSprite;

    [Header("Durability")]
    public bool noDurability;
    public int dura;

    [Header("Collectibles")]
    public GameObject collectiblesItem;
    public int maxDrop;
}
