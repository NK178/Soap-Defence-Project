using UnityEngine;

[CreateAssetMenu(fileName = "ItemOutput", menuName = "Scriptable Objects/ItemOutput")]

// 4/9 
// now with this method, this should work in allowing me to dynamically input shop conditions 
// just need to change the template type then should work 

public class ItemOutput : ShopItemOutput<ShopItem>
{
    public ShopItem item;

    public override ShopItem GetOutput()
    {
        return item;
    }
}