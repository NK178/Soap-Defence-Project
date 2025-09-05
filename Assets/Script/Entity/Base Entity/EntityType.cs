using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "EntityType", menuName = "Scriptable Objects/EntityType")]
public class EntityType : ScriptableObject
{
    [SerializeField] protected ENTITYTYPE type; 
    
    public ENTITYTYPE GetEntityType()
    {
        return type;
    }
}
