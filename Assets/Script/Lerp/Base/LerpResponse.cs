using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LerpResponse : ScriptableObject
{
    //store the lerp type so that the manager can use 
    public StringSO lerpType;
    public abstract void HandleResponse(GameObject reference);
    public abstract void AddValueToCalculation(ILerpData lerpData);

}
