using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.UIElements;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private FloatSO playerMoney;
    [SerializeField] private CheckColliderByTag mouseCollisionRef;
    [SerializeField] private OnMouseInteracts mouseReference;
    [SerializeField] private float maxMoney;
    [SerializeField] private float startingCash;
    [SerializeField] private float bubbleAddAmt;
    [SerializeField] private List<ShopItem> itemList;
    [SerializeField] private List<GameObject> UIGrid;

    private ShopItem selectedItem;
    public static ShopManager instance { get; private set; }
    [HideInInspector] public bool isDragging;


    private bool isEmpty = true;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEmpty)
        {
            HandleUI();
        }
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

    //check if valid buy 
    public void HandleDragStart(ShopItem item, PointerEventData eventData)
    {

        Debug.Log($"Started dragging {item.name}");
        //check price 
        if (item.CheckIfCanBuy())
        {
            selectedItem = item;
            isDragging = true;
        }
    }

    //this one is handled by the mouse functions not drag drop 
    public void HandleDragEndByMouse()
    {

        bool shouldResolve = false;
        object newPurchase = null;
        if (selectedItem != null)
        {
            Debug.Log($"Dropped {selectedItem.name}");
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
                    Debug.Log("TYPE ENTITY");
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


    ////complete the purchase 
    //public void HandleDragEnd(ShopItem item, PointerEventData eventData)
    //{
    //    Debug.Log($"Dropped {item.name}");
    //    bool shouldResolve = false;
    //    object newPurchase = null;
    //    if (item.CheckIfCanBuy())
    //    {
    //        newPurchase = item.Buy();
    //        if (newPurchase != null)
    //            shouldResolve = true;
    //    }

    //    //manage dropping 
    //    bool succesfulDrop = false;
    //    if (shouldResolve)
    //    {
    //        switch (newPurchase)
    //        {
    //            //apparenlty can do this and switch the typing so now entity got all the newpurchase stuff wow 
    //            case Entity entity:
    //                Debug.Log("TYPE ENTITY");
    //                succesfulDrop = HandleEntityDrop(entity); 
    //                break;
    //            default:
    //                newPurchase = null;
    //                succesfulDrop = false;
    //                break;
    //        }
    //    }

    //    //manage money 
    //    if (succesfulDrop)
    //    {
    //        //excute all respective conditions 
    //        item.HandleConditionResponse();
    //    }

    //    selectedItem = null;
    //    isDragging = false;
    //}


    bool HandleEntityDrop(Entity entity)
    {

        bool validDrop = false;
        //pls work I beg u
        GameObject target = mouseReference.currentTarget;
        if (target != null)
        {
            if (target.transform.childCount == 0)
                validDrop = true;
        }

        if (validDrop)
        {
            Debug.Log("VALID DROP");
            Vector3 gridPosition = target.transform.position;
            GameObject newEntity = Instantiate(entity.gameObject, gridPosition, transform.rotation);
            //make the entity be a child of the gameobejct 
            newEntity.transform.SetParent(target.transform);
            validDrop = true;
        }
        else
            Debug.Log("INVALID DROP");
        return validDrop;


        //bool validDrop = false;
        ////pls work I beg u
        //if (mouseCollisionRef.currentColliding != null)
        //{
        //    if (mouseCollisionRef.currentColliding.transform.childCount == 0)
        //        validDrop = true;
        //}

        //if (validDrop)
        //{
        //    Debug.Log("VALID DROP");
        //    Vector3 gridPosition = mouseCollisionRef.currentColliding.transform.position;
        //    GameObject newEntity = Instantiate(entity.gameObject, gridPosition, transform.rotation);
        //    //make the entity be a child of the gameobejct 
        //    newEntity.transform.SetParent(mouseCollisionRef.currentColliding.transform);
        //    validDrop = true;
        //}
        //else
        //    Debug.Log("INVALID DROP");
        //return validDrop;
    }

    public void HandleBubbleCollection()
    {
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



}
