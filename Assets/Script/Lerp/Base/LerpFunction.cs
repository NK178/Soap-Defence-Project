using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "LerpFunction", menuName = "Scriptable Objects/LerpFunction")]
public abstract class LerpFunction : ScriptableObject
{
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public LERPTYPE lerpType;


    public ILerpData lerpData;

    public abstract void Init(GameObject reference);
    public abstract void Reset();
    public abstract IEnumerator ExcuteLerp(GameObject reference);



}
