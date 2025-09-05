using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemPriceCondition", menuName = "Scriptable Objects/ShopItemPriceCondition")]
public class ShopItemPriceCondition : ShopItemCondition
{
    public float price; 
    public override bool IsValid()
    {

        if (ShopManager.instance != null)
        {
            float currentMoney = ShopManager.instance.GetCurrentMoney();
            float leftover = currentMoney - price;
            if (leftover > 0)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }
}
