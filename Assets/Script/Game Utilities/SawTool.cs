using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class SawTool : MonoBehaviour, InterfaceDragHandler, IPointerClickHandler
{
    [SerializeField] private OnMouseInteracts mouseReference;
    bool isSawActive;
    bool isActive;

    void Awake()
    {
        isSawActive = false;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActive)
            return;
        isSawActive = true;
        Debug.Log("SHOVEL STATUS " + isSawActive);
    }

    public void OnDrag(PointerEventData eventData)
    {
 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isActive)
            return;
        Entity target = mouseReference.currentTarget.GetComponent<Entity>();
        if (target != null)
        {
            Destroy(target);
        }
        isSawActive = false;
    }

    public void HandleShovelMouseRelease()
    {
        if (!isSawActive || !isActive)
            return;
        Entity target = mouseReference.currentTarget.GetComponent<Entity>();
        if (target != null)
        {
            Destroy(target.gameObject);
        }
        isSawActive = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSawActive = !isSawActive;
        Debug.Log("SHOVEL STATUS " + isSawActive);
    }

    public bool GetSawActiveStatus()
    {
        return isSawActive;
    }

    public void SetSawActive(bool condition)
    {
        isSawActive = condition;
    }

    public void ToggleSaw()
    {
        isSawActive = !isSawActive;
    }
}
