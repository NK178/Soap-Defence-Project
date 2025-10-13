using UnityEngine;

public class RequireParentReference : MonoBehaviour
{ 
    private Entity referenceEntity;
    private Entity targetEntity; 


    public void SetReferenceEntity(Entity reference)
    {
        referenceEntity = reference; 
    }

    public Entity GetReferenceEntity()
    {
        return referenceEntity;
    }

    public void SetTargetEntity(Entity reference)
    {
        targetEntity = reference;
    }

    public Entity GetTargetEntity()
    {
        return targetEntity;
    }
}
