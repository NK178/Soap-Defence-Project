using NUnit.Framework.Constraints;
using UnityEngine;

public class TypeInteractionData : MonoBehaviour
{
    //testing 
    private Entity referenceEntity; 

    //[SerializeField] private EntityType baseType;  
    //public EntityType GetBaseType()
    //{
    //    return baseType;
    //}

    //public void SetBaseType(EntityType newType)
    //{
    //    baseType = newType;
    //}

    public void SetReferenceEntity(Entity reference)
    {
        referenceEntity = reference; 
    }

    public Entity GetReferenceEntity()
    {
        return referenceEntity;
    }
}
