using System.Collections;
using UnityEngine;

public abstract class EntityFunctions : ScriptableObject
{
    //public abstract void Excute();


    [HideInInspector] public IEnumerator coroutine;

    private void OnEnable()
    {
        coroutine = ExcuteCoroutine();
    }

    public abstract IEnumerator ExcuteCoroutine();
}
