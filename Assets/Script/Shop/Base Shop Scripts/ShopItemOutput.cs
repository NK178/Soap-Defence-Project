using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemOutput", menuName = "Scriptable Objects/ShopItemOutput")]
//Note 4/9: was tryna figure out some better way to implement this but this should work for now 



//weird work around where I make a non generic version of this class so that the inspector dont get mad at me 
public abstract class ShopItemOutput : ScriptableObject
{
    public abstract object GetOutputObject();
}

//actual generic version that will be used 
public abstract class ShopItemOutput<T> : ShopItemOutput
{
    public abstract T GetOutput();

    public override object GetOutputObject()
    {
        return GetOutput();
    }

}
