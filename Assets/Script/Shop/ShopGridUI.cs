using UnityEngine;
using UnityEngine.EventSystems;

public class ShopGridUI : MonoBehaviour, AbstractDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Pls work I beg u ");
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
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
