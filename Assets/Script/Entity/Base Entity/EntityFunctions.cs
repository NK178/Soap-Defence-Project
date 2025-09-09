using System.Collections;
using UnityEngine;

public abstract class EntityFunctions : ScriptableObject
{
    public abstract IEnumerator ExcuteCoroutine(GameObject parentObject = null);

}
