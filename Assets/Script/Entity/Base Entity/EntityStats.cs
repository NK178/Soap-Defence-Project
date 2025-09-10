using System.Data;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;



[CreateAssetMenu(fileName = "EntityStats", menuName = "Scriptable Objects/EntityStats")]
public class EntityStats : ScriptableObject
{

    [SerializeField] private float value;
    [SerializeField] private STATSTYPE statType;

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public float GetValue()
    {
        return value;
    }

    public STATSTYPE GetStatType()
    {
        return statType;
    }
}
