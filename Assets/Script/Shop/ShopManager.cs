using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI; 


public class ShopManager : MonoBehaviour, IBeginDragHandler
{

    [SerializeField] private List<ShopItem> itemList;
    [SerializeField] private List<GameObject> UIGrid;


    private bool isEmpty = true;


    private void OnEnable()
    {
        if (itemList.Count > 0)
            isEmpty = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEmpty)
        {
            HandleUI();
        }
    }


    void HandleUI()
    {
        //safety check 
        if (UIGrid.Count < itemList.Count)
            return;

        for (int iter = 0; iter < itemList.Count; iter++)
        {

            //4/9 cheap method may change later 
            GeneralUI gridUI = UIGrid[iter].GetComponentInChildren<GeneralUI>();
            if (gridUI != null)
                gridUI.GetImage().sprite = itemList[iter].GetSprite();

        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
