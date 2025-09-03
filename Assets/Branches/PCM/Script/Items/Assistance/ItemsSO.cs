using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsSO", menuName = "Scriptable Objects/ItemsSO")]
public class ItemsSO : ScriptableObject
{
    public Sprite image;
    public GameObject itemPrefab;
    public int money;
    
}
