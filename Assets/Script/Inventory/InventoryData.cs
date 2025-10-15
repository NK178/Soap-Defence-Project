using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    //since I already have shop item stuff i will just reuse them
    public List<AvailableItem> itemList; 
}

[System.Serializable]
public class AvailableItem {
    public bool isUnlocked;
    public ShopItem item; 
}