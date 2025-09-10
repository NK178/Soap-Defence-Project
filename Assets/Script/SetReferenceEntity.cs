using UnityEngine;

public class RequireParentReference : MonoBehaviour
{ 
    private Entity referenceEntity; 

    public void SetReferenceEntity(Entity reference)
    {
        referenceEntity = reference; 
    }

    public Entity GetReferenceEntity()
    {
        return referenceEntity;
    }
}
