    using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveInDirection", menuName = "Scriptable Objects/MoveInDirection")]
public class MoveInDirection : EntityFunctions
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float moveSpeed;

    public override IEnumerator ExcuteCoroutine(GameObject parentObject = null)
    {
        while (true)
        {
            Vector3 prevPos = parentObject.transform.position;
            Vector3 velocity = direction * moveSpeed * Time.deltaTime;
            Vector3 newPos = prevPos + velocity;
            parentObject.transform.position = newPos;
            yield return null;
        }
    }
}
