using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DoNothingFunction", menuName = "Scriptable Objects/DoNothingFunction")]
public class DoNothingFunction : EntityFunctions
{

    //literally do nothing 
    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        yield return null;
    }


    public override void CopyData(EntityFunctions reference)
    {
        // do nothing lmao 
    }

}
