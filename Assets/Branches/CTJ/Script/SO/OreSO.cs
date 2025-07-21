using UnityEngine;

[CreateAssetMenu(fileName = "OreSO", menuName = "Scriptable Objects/OreSO")]
public class OreSO : ScriptableObject
{
    public string oreName;
    public Sprite OreSprite;

    [Header("Durability")]
    public bool noDurability;
    public int lv1Dura;
    public int lv2Dura;
    public int lv3Dura;

    [Header("Collectibles")]
    public GameObject collectiblesItem;
    public int maxDrop;
}
