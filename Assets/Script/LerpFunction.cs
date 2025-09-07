using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "LerpFunction", menuName = "Scriptable Objects/LerpFunction")]
public abstract class LerpFunction : ScriptableObject
{

    public abstract void Init();
    public abstract void Reset();
    public abstract IEnumerator ExcuteLerp(GameObject reference);

    [HideInInspector] public bool isActive = false;
}
