using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "LerpFunction", menuName = "Scriptable Objects/LerpFunction")]
public abstract class LerpFunction : ScriptableObject
{
    public StringSO lerpType;
    public ILerpData lerpData;
    [HideInInspector] public bool isActive = false;
    public abstract void Init(GameObject reference);
    public abstract void Reset();
    public abstract IEnumerator ExcuteLerp(GameObject reference);



}
