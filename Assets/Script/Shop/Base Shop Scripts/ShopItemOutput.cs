using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemOutput", menuName = "Scriptable Objects/ShopItemOutput")]

//using generics so that I can decalre any return type I want 
public abstract class ShopItemOutput<T> : ScriptableObject, IShopItemOutput
{
    public abstract T GetOutputType();

    //need to put this here because class takes from interface 
    public abstract void GetOutput(); 
}

public interface IShopItemOutput
{
    void GetOutput();
}



