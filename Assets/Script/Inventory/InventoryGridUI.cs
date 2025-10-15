using UnityEngine;
using UnityEngine.EventSystems;


//inherit from base class
public class InventoryGridUI : GeneralUI, InterfaceDragHandler, IPointerClickHandler
{
    private ShopItem item;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (ShopManager.instance != null)
            ShopManager.instance.AddItemIntoList(item);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //ShopManager.instance.HandleDragStart(item, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }


    public void SetItem(ShopItem item)
    {
        this.item = item;
    }

    public ShopItem GetItem()
    {
        return item;
    }

}
