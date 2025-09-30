using UnityEngine;
using System.Collections;
using TMPro;


[CreateAssetMenu(fileName = "CooldownTimer", menuName = "Scriptable Objects/CooldownTimer")]
public class CooldownTimer : EntityStateDecision
{
    [SerializeField] private float coolDownTime;
    bool isCoolDownDone = false;



    public override bool DecisionCheck(Entity entity)
    {
        //Debug.Log("TIMER BOOL : " + isCoolDownDone);
        if (isCoolDownDone)
        {
            isCoolDownDone = false;
            return true;
        }
        else
            return false;
    }

    public override IEnumerator ExcuteCoroutine(Entity entity)
    {
        //Debug.Log("TIMER RUNNING");
        yield return new WaitForSeconds(coolDownTime);
        isCoolDownDone = true;
        //Debug.Log("TIMER HAS RAN DOWN");
    }

    public override void CopyData(EntityStateDecision reference)
    {
        if (reference is CooldownTimer CDTimer)
        {
            coolDownTime = CDTimer.coolDownTime;
            isCoolDownDone = CDTimer.isCoolDownDone;
        }
    }
}
