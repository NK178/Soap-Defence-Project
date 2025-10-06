    using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInDirection", menuName = "Scriptable Objects/MoveInDirection")]
public class MoveInDirection : EntityFunctions
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float referenceMoveSpeed;

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        string velocityDataKey = this.name + "_velocity";
        string speedModifierDataKey = this.name + "_speedModifier";
        while (true)
        {
            //test this with water puddle 
            float speedModifier = 1f;
            if (entity.dataLibrary.CheckIfKeyExists(speedModifierDataKey, DATATYPE.FLOAT))
                speedModifier = entity.dataLibrary.GetFloat(speedModifierDataKey);
            else
                entity.dataLibrary.AddFloat(speedModifierDataKey, speedModifier);
            float actualMoveSpeed = referenceMoveSpeed * speedModifier;
            Debug.Log("CURRENT MOVE SPEED: " + actualMoveSpeed);
            Vector3 prevPos = entity.gameObject.transform.position;
            Vector3 velocity = direction * actualMoveSpeed * Time.deltaTime;
            Vector3 newPos = prevPos + velocity;
            entity.gameObject.transform.position = newPos;

            //store velocity for calculations yay 
            Vector3 test = direction * referenceMoveSpeed; 
            entity.dataLibrary.AddVector3(velocityDataKey, test);
            yield return null;
        }
    }
}
