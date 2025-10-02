using UnityEngine;
using System.Collections;
using TMPro;
using UnityEditor.Search;


[CreateAssetMenu(fileName = "CooldownTimer", menuName = "Scriptable Objects/CooldownTimer")]
public class CooldownTimer : EntityStateDecision
{
    [SerializeField] private float coolDownTime;  


    public override bool DecisionCheck(Entity entity)
    {
        string dataKey = $"{this.name}_bool";
        bool isCoolDownDone = entity.dataLibrary.GetBool(dataKey);
        if (isCoolDownDone)
        {
            //reset 
            entity.dataLibrary.SetBool(dataKey, false);
            return true;
        }
        else
            return false;

    }

    public override IEnumerator ExcuteCoroutine(Entity entity)
    {
        string dataKey = $"{this.name}_bool";

        if (entity.dataLibrary == null)
        {
            Debug.LogError("DataLibrary is null on entity: " + entity.name);
            yield break;
        }
        //check whether exists already handled in datalibrary itself
        entity.dataLibrary.AddBool(dataKey, false);
        yield return new WaitForSeconds(coolDownTime);
        entity.dataLibrary.SetBool(dataKey, true);
    }

}
