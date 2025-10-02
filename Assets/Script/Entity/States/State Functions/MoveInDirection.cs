    using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInDirection", menuName = "Scriptable Objects/MoveInDirection")]
public class MoveInDirection : EntityFunctions
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float moveSpeed;

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        string dataKey = this.name + "_velocity";
        while (true)
        {
            Vector3 prevPos = entity.gameObject.transform.position;
            Vector3 velocity = direction * moveSpeed * Time.deltaTime;
            Vector3 newPos = prevPos + velocity;
            entity.gameObject.transform.position = newPos;

            //store velocity for calculations yay 
            Vector3 test = direction * moveSpeed; 
            entity.dataLibrary.AddVector3(dataKey, test);
            yield return null;
        }
    }
}
