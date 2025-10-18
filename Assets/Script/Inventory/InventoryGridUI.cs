using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;


//inherit from base class
public class InventoryGridUI : GeneralUI, InterfaceDragHandler, IPointerClickHandler
{
    [SerializeField] private float selectedColour;
    [SerializeField] private Image baseImage; 
    [SerializeField] private Image pictureImage; 
    private ShopItem item;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (ShopManager.instance != null)
        {   
            ShopManager.instance.AddItemIntoList(item);
            ChangeImageOpacity(selectedColour);
        }
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

    public void ChangeImageOpacity(float alpha)
    {
        Color baseImageNewColour = baseImage.color;
        Color pictureImageNewColour = pictureImage.color;
        pictureImageNewColour.a = baseImageNewColour.a = alpha;
        baseImage.color = baseImageNewColour;
        pictureImage.color = pictureImageNewColour;
    }

    public void SetItem(ShopItem item)
    {
        this.item = item;
    }

    public ShopItem GetItem()
    {
        return item;
    }

    public float GetBaseOpacity()
    {
        return selectedColour;
    }

}
