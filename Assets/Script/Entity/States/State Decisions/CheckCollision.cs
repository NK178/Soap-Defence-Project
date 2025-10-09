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
        bool result = false;
        bool validCollider = false;
        if (collider != null)
            validCollider = true;

        if (validCollider)
        {
            for (int iter = 0; iter < collider.allColliding.Count; iter++)
            {
                if (collider.allColliding[iter].gameObject.tag == tagName)
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }
}
