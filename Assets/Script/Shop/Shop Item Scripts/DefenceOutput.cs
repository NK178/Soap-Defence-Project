using UnityEngine;

[CreateAssetMenu(fileName = "DefenceOutput", menuName = "Scriptable Objects/DefenceOutput")]
public class DefenceOutput : ShopItemOutput<Entity>
{
    [SerializeField] private Entity defence; 
    public override Entity GetOutput()
    {
        return defence; 
    }
}
