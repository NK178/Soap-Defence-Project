using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "FusionDecision", menuName = "Scriptable Objects/FusionDecision")]
abstract public class FusionDecision<T> : ScriptableObject
{
    public abstract bool IsFusionValid(List<T> recipe, List<T> fuseInput);
}
