using UnityEngine.EventSystems;
using UnityEngine;

//inherit from base class
public class ShopGridUI : GeneralUI, InterfaceDragHandler, IPointerClickHandler
{
    private ShopItem item;



    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        ShopManager.instance.HandleDragStart(item, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ShopManager.instance.CancelDrag();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //15/10 method not so good, can consider using shop manager's activty status to compare instead 
        //for now, this only deals with the returning back to inventory thingy, I didnt handle 
        if (GameManager.instance != null)
        {
            if (GameManager.instance.GetCurrentGameState() == GAMESTATES.MENU && item != null)
            {
                //I have no good way to handle this in the inventory 
                //best bet is to send back the item reference from gameManager

                ShopManager.instance.RemoveItemFromShop(item);
                Debug.Log("Send it back boi");
            }

        }
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
