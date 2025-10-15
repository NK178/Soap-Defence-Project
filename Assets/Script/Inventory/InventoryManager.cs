using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private InventoryData inventoryData;
    private List<InventoryGridUI> inventoryUIList;
    private bool isTriggered;

    private bool isActive;
    public static int totalAvailableDefences; 
    void Awake()
    {
        totalAvailableDefences = 0;
        inventoryUIList = new List<InventoryGridUI>();
        LoadItems();
        isTriggered = isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return; 

        if (isTriggered)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for(int iter = 0; iter < inventoryUIList.Count; iter++)
        {
            if (inventoryUIList[iter] != null)
            {
                inventoryUIList[iter].GetImage().sprite = inventoryUIList[iter].GetItem().GetSprite();
            }
        }
    }

    void LoadItems()
    {
        if (inventoryData == null)
            return; 

        for (int iter = 0; iter < inventoryData.itemList.Count; iter++)
        {
            if (inventoryData.itemList[iter].isUnlocked && inventoryData.itemList[iter].item != null)
            {
                GameObject newItemUI = Instantiate(itemPrefab);
                InventoryGridUI inventoryItemUI = newItemUI.GetComponent<InventoryGridUI>();
                inventoryItemUI.SetItem(inventoryData.itemList[iter].item);
                inventoryItemUI.gameObject.transform.SetParent(transform);
                inventoryUIList.Add(inventoryItemUI);
                totalAvailableDefences++;
            }
        }
    }





}
