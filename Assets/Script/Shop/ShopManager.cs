using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.UIElements;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private FloatSO playerMoney;
    [SerializeField] private float maxMoney;
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
        if (item.CheckIfCanBuy())
        {
            item.Buy();
            selectedItem = null;
        }
        isDragging = false;
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
