using UnityEngine.EventSystems;


//inherit from base class
public class ShopGridUI : GeneralUI, InterfaceDragHandler
{
    private ShopItem item;



    public void OnBeginDrag(PointerEventData eventData)
    {
        ShopManager.instance.HandleDragStart(item, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //ShopManager.instance.HandleDragEnd(item, eventData);
    }

    public void SetItem(ShopItem item)
    {
        this.item = item;
    }

}
