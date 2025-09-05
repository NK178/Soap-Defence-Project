using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;


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
        Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ShopManager.instance.HandleDragEnd(item, eventData);
    }

    public void SetItem(ShopItem item)
    {
        this.item = item;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
