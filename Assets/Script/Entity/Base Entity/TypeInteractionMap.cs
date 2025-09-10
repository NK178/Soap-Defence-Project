using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TypeInteractions {
    public EntityType baseType;  
    public EntityType strongAgainst;  
    public EntityType weakAgainst;  
}

public class TypeInteractionMap : MonoBehaviour {

    [SerializeField] private List<TypeInteractions> interactionsList;
    public static TypeInteractionMap instance { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public TypeInteractions GetTypeInteraction(EntityType type)
    {
        foreach (TypeInteractions typeInteraction in interactionsList)
        {
            if (typeInteraction.baseType.GetEntityType() == type.GetEntityType())
            {
                return typeInteraction;
            }
        }
        return null;
    }
}
