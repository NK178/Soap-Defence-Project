    using System.Collections;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInDirection", menuName = "Scriptable Objects/MoveInDirection")]
public class MoveInDirection : EntityFunctions
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float moveSpeed;


    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        while (true)
        {
            Vector3 prevPos = entity.gameObject.transform.position;
            Vector3 velocity = direction * moveSpeed * Time.deltaTime;
            Vector3 newPos = prevPos + velocity;
            entity.gameObject.transform.position = newPos;
            yield return null;
        }
    }

    public override void CopyData(EntityFunctions reference)
    {
        if (reference is MoveInDirection MoveDirection)
        {
            direction = MoveDirection.direction;
            moveSpeed = MoveDirection.moveSpeed;
        }
    }

}
