using UnityEngine;

[CreateAssetMenu(fileName = "EntityNameList", menuName = "Scriptable Objects/EntityNameList")]
public class EntityNameList : ScriptableObject
{

    //used to sort entities easier 
    public enum ENTITYNAME { 
        SOAPKETTLE, 
        SOAPBAR,
        WATERPUDDLE, 
        SOAPYPUDDLE,
        GREASE, //shoudl rethink this one 
        NUM_NAMES
    }

}
