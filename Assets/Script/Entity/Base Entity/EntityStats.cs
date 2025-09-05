using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class EntityStats : ScriptableObject
{
    protected float value;
    public abstract float GetValue();
    public abstract void  SetValue(float newValue);
}
