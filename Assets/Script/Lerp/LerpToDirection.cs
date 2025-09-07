using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "LerpToDirection", menuName = "Scriptable Objects/LerpToDirection")]
public class LerpToDirection : LerpFunction
{
    private enum LERP_DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NUM_DIRECTION
    }

    [SerializeField] private LERP_DIRECTION directionToLerp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime = 1f;

    private LERP_DIRECTION defaultDirection;
    private float defaultMoveSpeed;
    private float currentSpeed = 0f;



    public override void Init(GameObject reference)
    {
        if (!isActive)
        {
            defaultMoveSpeed = moveSpeed;
            defaultDirection = directionToLerp;
            currentSpeed = 0f;
            isActive = true;
        }
    }

    public override void Reset()
    {
        if (isActive)
        {
            moveSpeed = defaultMoveSpeed;
            directionToLerp = defaultDirection;
            currentSpeed = 0f;
            isActive = false;
        }
    }

    public override IEnumerator ExcuteLerp(GameObject reference)
    {
        while (isActive)
        {
            Rigidbody2D rb = reference.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 moveDirection = GetDirection();
                currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime / accelerationTime);
                Vector2 targetVelocity = moveDirection * currentSpeed;
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 5f);
                yield return null;
            }
        }

        ///////////////////////////////////  method one that works 
        //currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime / accelerationTime);
        //Vector3 moveDirection = GetDirection();
        //reference.transform.position += moveDirection * currentSpeed * Time.deltaTime;
        //yield return null;
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;
        switch (directionToLerp)
        {

            case LERP_DIRECTION.UP:
                direction = Vector2.up;
                break;
            case LERP_DIRECTION.DOWN:
                direction = Vector2.down;
                break;
            case LERP_DIRECTION.LEFT:
                direction = Vector2.left;
                break;
            case LERP_DIRECTION.RIGHT:
                direction = Vector2.right;
                break;
        }
        return direction;
    }


}
