using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private FloatSO playerMoney;
    [SerializeField] private OnMouseInteracts mouseReference;
    [SerializeField] private float maxMoney;
    [SerializeField] private float startingCash;
    [SerializeField] private float bubbleAddAmt;
    [SerializeField] private List<ShopItem> itemList;
    [SerializeField] private List<GameObject> UIGrid;

    private ShopItem selectedItem;
    public static ShopManager instance { get; private set; }
    [HideInInspector] public bool isDragging;
    [HideInInspector] public bool areDefencesEnough;

    private bool canCollectBubble; 
    private bool isEmpty = true;
    private bool isActive;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (itemList.Count > 0)
            isEmpty = false;
        selectedItem = null;

        playerMoney.value = startingCash;
        if (playerMoney.value > maxMoney)
            playerMoney.value = maxMoney;


        isActive = true;
        canCollectBubble = true;
        areDefencesEnough = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isActive)
        //    return; 


        if (!isEmpty)
        {
            HandleUI();
        }
        areDefencesEnough = DidPlayerBringEnoughDefences();
    }


    private bool DidPlayerBringEnoughDefences()
    {
        if (itemList == null)
            return false;
        //if 8 for 8 
        else if (itemList.Count == UIGrid.Count)
            return true;
        //if total items more than total shop space (8), player must bring all 8 
        else if (InventoryManager.totalAvailableDefences >= UIGrid.Count)
            return false;
        //if total items NOT more sthan total shop space (8), player brings at least 1 defence
        else if (itemList.Count > 0)
            return true;
        //no defence at all 
        else 
            return false; 
    }

    


    void HandleUI()
    {
        //safety check 
        if (UIGrid.Count < itemList.Count)
            return;

        for (int iter = 0; iter < itemList.Count; iter++)
        {

            //4/9 cheap method may change later 
            ShopGridUI gridUI = UIGrid[iter].GetComponentInChildren<ShopGridUI>();
            if (gridUI != null)
            {
                gridUI.GetImage().sprite = itemList[iter].GetSprite();
                gridUI.SetItem(itemList[iter]);
            }
        }

    }

    public void AddItemIntoList(ShopItem newItem)
    {
        if (itemList == null)
            itemList = new List<ShopItem>();

        //check for duplicates 
        bool shoudlAdd = true;
        for (int iter = 0; iter < itemList.Count; iter++)
        {
            // ERROR HERE LOADING MULTIPLE ITEMS, PROBABLY GET OUTPUT NULL 
            if (itemList[iter].GetOutput().name == newItem.GetOutput().name)
            {
                shoudlAdd = false;
                break;
            }
        }

        if (shoudlAdd)
        {
            itemList.Add(newItem);
            isEmpty = false;
        }
        else
            Debug.Log("ADD ITEM FAILED");

    }

    //check if valid buy 
    public void HandleDragStart(ShopItem item, PointerEventData eventData)
    {
        if (!isActive || item == null)
            return;
        //Debug.Log($"Started dragging {item.name}");
        //check price 
        if (item.CheckIfCanBuy())
        {
            selectedItem = item;
            isDragging = true;
        }

    }

    public void CancelDrag()
    {
        if (!isActive || selectedItem == null)
            return;
        selectedItem = null;
        isDragging = false;
    }

    //this one is handled by the mouse functions not drag drop 
    public void HandleDragEndByMouse()
    {

        bool shouldResolve = false;
        object newPurchase = null;
        if (selectedItem != null)
        {
            //Debug.Log($"Dropped {selectedItem.name}");
            if (selectedItem.CheckIfCanBuy())
            {
                newPurchase = selectedItem.Buy();
                if (newPurchase != null)
                    shouldResolve = true;
            }
        }

        //manage dropping 
        bool succesfulDrop = false;
        if (shouldResolve)
        {
            switch (newPurchase)
            {
                //apparenlty can do this and switch the typing so now entity got all the newpurchase stuff wow 
                case Entity entity:
                    //Debug.Log("TYPE ENTITY");
                    succesfulDrop = HandleEntityDrop(entity);
                    break;
                default:
                    newPurchase = null;
                    succesfulDrop = false;
                    break;
            }
        }

        //manage money 
        if (succesfulDrop)
        {
            //excute all respective conditions 
            selectedItem.HandleConditionResponse();
        }

        selectedItem = null;
        isDragging = false;
    }

    public bool HandleEntityDrop(Entity refEntity)
    {
        bool validDrop = false;

        //pls work I beg u
        GameObject target = mouseReference.currentTarget;
        if (target == null)
            return false;

        //Debug.Log("TARGET NAME " + target.gameObject.name);

        //Case if drop onto another entity for fusion 
        if (target.GetComponent<Entity>() != null)
        {
            //Get colliding entity 
            Entity entity2 = target.GetComponent<Entity>();
            List<Entity> validFusionList = new List<Entity> { refEntity, entity2 };

            //not all entity drops may be valid 
            validDrop = FusionManager.instance.TriggerFusionIfValid(validFusionList);
        }
        else if (target.transform.childCount == 0)
        {
            Vector3 gridPosition = target.transform.position;
            GameObject newEntity = Instantiate(refEntity.gameObject, gridPosition, transform.rotation);
            newEntity.transform.localScale = new Vector3(1, 1, 1);
            //make the entity be a child of the gameobejct 
            newEntity.transform.SetParent(target.transform);
            validDrop = true;
        }

        //Debug.Log("DROP STATUS: " + validDrop);

        return validDrop;
    }

    public void HandleBubbleCollection()
    {
        //if (!isActive)
        //    return;
        if (!canCollectBubble)
            return; 
        AddMoney(bubbleAddAmt);
        if (mouseReference.currentTarget != null)
        {
            Destroy(mouseReference.currentTarget);
        }
    }

    public ShopItem GetCurrentItem()
    {
        if (selectedItem != null)
            return selectedItem;
        else
            return null;
    }

    public void ReduceMoney(float amount)
    {
        playerMoney.value -= amount;
        if (playerMoney.value < 0)
            playerMoney.value = 0;
    }

    public void AddMoney(float amount)
    {
        playerMoney.value += amount;
        if (playerMoney.value > maxMoney)
            playerMoney.value = maxMoney;
    }

    public float GetCurrentMoney()
    {
        return playerMoney.value;
    }

    public void SetActiveStatus(bool condition)
    {
        isActive = condition;
    }

    public void SetBubbleCollectionStatus(bool condition)
    {
        canCollectBubble = condition;
    }

}
