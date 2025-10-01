using UnityEngine;
using System.Collections;
using TMPro;


[CreateAssetMenu(fileName = "CooldownTimer", menuName = "Scriptable Objects/CooldownTimer")]
public class CooldownTimer : EntityStateDecision
{
    [SerializeField] private float coolDownTime;  

    public override bool DecisionCheck(Entity entity)
    {
        bool isCoolDownDone = entity.dataLibrary.GetBool(GetInstanceID());
        if (isCoolDownDone)
        {
            //reset 
            entity.dataLibrary.SetBool(GetInstanceID(), false);
            return true;
        }
        else
            return false;
        
    }

    public override IEnumerator ExcuteCoroutine(Entity entity)
    {
        if (entity.dataLibrary == null)
        {
            Debug.LogError("DataLibrary is null on entity: " + entity.name);
            yield break;
        }

        //check whether exists already handled in datalibrary itself
        entity.dataLibrary.AddBool(GetInstanceID(), false);
        yield return new WaitForSeconds(coolDownTime);
        entity.dataLibrary.SetBool(GetInstanceID(), true);
    }

}
