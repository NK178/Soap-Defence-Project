using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

//[CreateAssetMenu(fileName = "LerpFunction", menuName = "Scriptable Objects/LerpFunction")]
public abstract class LerpFunction : ScriptableObject
{
    public StringSO lerpType;
    public ILerpData lerpData;
    [HideInInspector] public bool isActive = false;
    public abstract void Init(GameObject reference);
    public abstract void Reset();
    public abstract IEnumerator ExcuteLerp(GameObject reference);

    public abstract void CopyClassData(LerpFunction reference);

    //public void CopyClassData(LerpFunction reference)
    //{
    //    //incase the function hasnt been init yet 
    //    lerpType = reference.lerpType;
    //    lerpData = reference.lerpData; 
    //}


}
