using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemCondition", menuName = "Scriptable Objects/ShopItemCondition")]
public abstract class ShopItemCondition : ScriptableObject
{
    public abstract bool IsValid(); 
    
}
