using UnityEngine;
using UnityEngine.EventSystems;


public interface AbstractDragHandler : IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData);

    public void OnDrag(PointerEventData eventData);

    public void OnEndDrag(PointerEventData eventData);
}
