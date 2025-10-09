using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

//I shoudl change this SO name to smth like entity raycast range check 
[CreateAssetMenu(fileName = "RaycastCheckInRange", menuName = "Scriptable Objects/RaycastCheckInRange")]
public class RaycastCheckInRange : EntityStateDecision
{

    [SerializeField] private string layerName;
    [SerializeField] private Vector2 raycastDirection;
    [SerializeField] private float raycastDistance;
    [SerializeField] private List<EntityType> ignoreType;


    public override bool DecisionCheck(Entity entity)
    {
        RaycastHit2D hit = Physics2D.Raycast(entity.gameObject.transform.position, raycastDirection, raycastDistance, LayerMask.GetMask(layerName));
        bool result = false;
        Entity target = null;
        if (hit.collider)
            target = hit.collider.gameObject.GetComponent<Entity>();

        if (target != null)
        {
            //Set true now 
            result = true;

            // if the target has one of the whitelisted types, return false 
            for (int iter = 0; iter < ignoreType.Count; iter++)
            {
                if (target.CheckIfThisEntityType(ignoreType[iter].GetEntityType()))
                    result = false;
            }
        }

        return result;
    }
}
