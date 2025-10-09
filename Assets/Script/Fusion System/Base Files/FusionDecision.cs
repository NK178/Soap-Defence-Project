using System.Collections.Generic;
using UnityEngine;

abstract public class FusionDecision<T> : ScriptableObject
{
    public abstract bool IsFusionValid(List<T> recipe, List<T> fuseInput);
}
