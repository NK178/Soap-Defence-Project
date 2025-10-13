using UnityEngine;
using UnityEngine.EventSystems;

public class WrenchTool : MonoBehaviour, InterfaceDragHandler, IPointerClickHandler
{
    [SerializeField] private OnMouseInteracts mouseReference; 
    Entity target;
    private bool isObjectPickedUp;
    private bool isWrenchActive;
    private bool isActive;

    void Awake()
    {
        target = null;
        isObjectPickedUp = isWrenchActive = false;
        isActive = true;
    }

    void Update()
    {
        if (!isActive)
        {
            isObjectPickedUp = isWrenchActive = false;
            target = null; 
        }
    }

    private bool IfMouseTargetIsEntity()
    {
        if (!isActive)
            return false;
        Entity reference = null;
        if (mouseReference.currentTarget != null)
        {
            reference = mouseReference.currentTarget.GetComponent<Entity>();
            if (reference != null)
                return true;
        }

        return false;
    }

    public void PickUpEntity()
    {
        if (!isActive)
            return; 
        if (!isObjectPickedUp && isWrenchActive)
        {
            if (IfMouseTargetIsEntity())
            {
                target = mouseReference.currentTarget.GetComponent<Entity>();
                isObjectPickedUp = true;
                Debug.Log("PICKED UP " + target.gameObject.name);
                //set animator here or handle in mouse handler 
            }
        }
    }

    public void HandleWrenchMouseRelease()
    {
        if (!isWrenchActive || !isActive)
            return; 

        //drop da entity 
        if (isObjectPickedUp)
        {
            Debug.Log("DROPPING " + target.gameObject.name);
            HandleDrop(target);
            isObjectPickedUp = false;
            isWrenchActive = false;
        }
        //pick up if nothing yet 
        else
        {
            PickUpEntity();
        }
    }

    private void HandleDrop(Entity reference)
    {
        GameObject target = mouseReference.currentTarget;
        if (target == null || !isActive)
            return;

        //to prevent unsafe deletions, I will just move the entity instead doing delete and spawning a new one like how shopmanager does 
        if (target.transform.childCount == 0)
        {
            Vector3 gridPosition = target.transform.position;
            reference.transform.position = gridPosition;
            reference.transform.SetParent(target.transform);
        }
        //check fusion 
        else if (target.GetComponent<Entity>() != null)
        {
            reference.SetActiveStatus(false);
            ShopManager.instance.HandleEntityDrop(reference);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActive)
            return;
        isWrenchActive = true;
        Debug.Log("WRENCH STATUS " + isWrenchActive);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isActive)
            return;
        //if hovering over entity let it be handled elsewhere 
        if (!IfMouseTargetIsEntity())
        {
            isWrenchActive = false;
            if (isObjectPickedUp)
            {
                isObjectPickedUp = false;
                target = null;
            }
            Debug.Log("WRENCH STATUS " + isWrenchActive);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isWrenchActive = !isWrenchActive;
        Debug.Log("WRENCH STATUS " + isWrenchActive);
    }

    public void ToggleWrench()
    {
        if (!isActive)
            return;
        isWrenchActive = !isWrenchActive;
        if (!isWrenchActive)
        {
            isObjectPickedUp = false;
            target = null;
        }
        Debug.Log("WRENCH STATUS " + isWrenchActive);
    }

    public Entity GetEntityReference()
    {
        return target; 
    }

    public bool GetWrenchActiveStatus()
    {
        return isWrenchActive;  
    }

    public bool IsObjectPickedUp()
    {
        return isObjectPickedUp;
    }

    public void SetWrenchActive(bool condition)
    {
        isWrenchActive = condition;
    }

    public void SetActiveStatus(bool condition)
    {
        isActive = condition;
    }

}
