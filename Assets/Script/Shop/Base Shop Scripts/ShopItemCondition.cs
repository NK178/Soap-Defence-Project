using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "ShopItemCondition", menuName = "Scriptable Objects/ShopItemCondition")]
public abstract class ShopItemCondition : ScriptableObject
{
    public abstract bool IsValid();

    //if action is taken after IsValid, response will be written here
    public abstract void ConditionResolve();
    
}
