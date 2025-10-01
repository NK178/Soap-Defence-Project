using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityStateDecision : ScriptableObject
{
    public abstract bool DecisionCheck(Entity entity);

    //not necessary to implement 
    virtual public IEnumerator ExcuteCoroutine(Entity entity)
    {
        yield break;
    }

}
