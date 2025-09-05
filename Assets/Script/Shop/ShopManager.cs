using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.UIElements;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private FloatSO playerMoney;
    [SerializeField] private CheckColliderByTag mouseCollisionRef;
    [SerializeField] private float maxMoney;
    [SerializeField] private float startingCash;
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

    //complete the purchase 
    public void HandleDragEnd(ShopItem item, PointerEventData eventData)
    {
        Debug.Log($"Dropped {item.name}");
        bool shouldResolve = false;
        object newPurchase = null;
        if (item.CheckIfCanBuy())
        {
            newPurchase = item.Buy();
            if (newPurchase != null)
                shouldResolve = true;
        }

        if (shouldResolve)
        {
            switch (newPurchase)
            {
                //apparenlty can do this and switch the typing so now entity got all the newpurchase stuff wow 
                case Entity entity:
                    HandleEntityDrop(entity); 
                    break;
                default:
                    newPurchase = null; 
                    break;
            }
        }

        selectedItem = null;
        isDragging = false;
    }


    void HandleEntityDrop(Entity entity)
    {

        //pls work I beg u
        Vector3 gridPosition = mouseCollisionRef.currentColliding.transform.position;
        GameObject newGameObject = Instantiate(entity.gameObject, gridPosition, transform.rotation);
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
}
