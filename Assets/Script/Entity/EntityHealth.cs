using UnityEngine;

[CreateAssetMenu(fileName = "EntityHealth", menuName = "Scriptable Objects/EntityHealth")]
public class EntityHealth : EntityStats
{

    [SerializeField] private float healthValue;

    private void Awake()
    {
        value = healthValue;
    }

    
    public override float GetValue()
    {
        return value;
    }

    public override void SetValue(float newValue)
    {
        value = newValue;
    }
}
