using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemPriceCondition", menuName = "Scriptable Objects/ShopItemPriceCondition")]
public class ShopItemPriceCondition : ShopItemCondition
{

    public FloatSO playerMoney;
    public float price; 
    public override bool IsValid()
    {
        float currentMoney = playerMoney.value; 
        float leftover = currentMoney - price;
        if (leftover > 0)
        {
            return true; 
        }
        else
            return false;
    }
}
