using System.Collections.Generic;
using UnityEngine;


abstract public class FusionResponse<T> : ScriptableObject
{
    public abstract void HandleFusionResponse(List<T> outputList, List<T> inputToResolveList); 
}
