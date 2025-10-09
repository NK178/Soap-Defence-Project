using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResetVector3Data", menuName = "Scriptable Objects/ResetVector3Data")]
public class ResetVector3Data : EntityFunctions
{
    [SerializeField] private string dataKey;
    [SerializeField] private Vector3 resetValue;


    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        //I guess only need call once unless weird stuff happens
        if (entity == null)
            yield return null;
        if (entity.dataLibrary.CheckIfKeyExists(dataKey,DATATYPE.VECTOR3))
            entity.dataLibrary.SetVector3(dataKey, resetValue);

    }
}
