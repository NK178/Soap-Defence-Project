using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

// a descions that requires to be a called manually by something like unity events 
[CreateAssetMenu(fileName = "CheckCollision", menuName = "Scriptable Objects/CheckCollision")]
public class CheckCollision : EntityStateDecision
{
    [SerializeField] private string tagName; 
    

    public override bool DecisionCheck(Entity entity)
    {
        CheckColliderByTag collider = entity.GetTagCollider();
        if (collider != null)
        {
            if (collider.gameObject.tag == tagName)
                return true;
            else
                return false;
        }
        return false;
    }
}
