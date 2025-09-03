using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemListSO", menuName = "Scriptable Objects/ItemListSO")]
public class ItemListSO : ScriptableObject
{
    public List<ItemsSO> List = new List<ItemsSO>(); 
}
