using UnityEngine;

[CreateAssetMenu(fileName = "EntityFusionDataWrapper", menuName = "Scriptable Objects/EntityFusionDataWrapper")]
public class EntityFusionDataWrapper : ScriptableObject, IFusionDataWrapper<Entity>
{
    /*reason why have data and fusion data, in a manager function assuming other types of data wrapper,
      they have a common property to call which is fusionData, if not then this interface is useless,
      also because I cant actually use fusion data to store the data, I have to make another variable for this 
    */




    [SerializeField] private ConcreteEntityFusionData data;
    public FusionData<Entity> fusionData
    {
        //essentially handling the response for fusionData 
        get => data;
        set => data = (ConcreteEntityFusionData)value;
    }

    
}

/* Unfortunate situation of Unity serialization where I need to create a concrete class in order to serialize, 
there is a workaround but its uncomfortable and requires alot of type handling that can go wrong*/

[System.Serializable]
public class ConcreteEntityFusionData: FusionData<Entity> { };