using System.Xml;
using UnityEngine;

[CreateAssetMenu(fileName = "RaycastCheckInRange", menuName = "Scriptable Objects/RaycastCheckInRange")]
public class RaycastCheckInRange : EntityStateDecision
{

    [SerializeField] private string layerName;
    [SerializeField] private Vector2 raycastDirection;
    [SerializeField] private float raycastDistance; 


    public override bool DecisionCheck(Entity entity)
    {
        RaycastHit2D hit = Physics2D.Raycast(entity.gameObject.transform.position, raycastDirection, raycastDistance, LayerMask.GetMask(layerName));
        if (hit.collider)
            return true;
        else
            return false;
    }
}
