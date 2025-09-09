using System.Collections;
using Unity.Properties;
using UnityEngine;


[CreateAssetMenu(fileName = "LerpBackAndForth", menuName = "Scriptable Objects/LerpBackAndForth")]
public class LerpBackAndForth : LerpFunction
{

    private enum OSCILLATION_AXIS
    {
        X_AXIS, 
        Y_AXIS,
    }

    private enum START_DIRECTION { 
        POSTIVE, 
        NEGATIVE
    }

    [SerializeField] private OSCILLATION_AXIS oscillation_axis;
    [SerializeField] private START_DIRECTION oscillation_startDirection;
    // lerp based on this
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;


    private float oscillationTime = 0f;
    private Vector3 startingPos;

    public override void Init(GameObject reference)
    {
        if (!isActive)
        {
            startingPos = reference.transform.position;
            isActive = true;
            lerpData = new LerpData<Vector3>();
        }
    }

    public override void Reset()
    {
        if (isActive)
        {
            isActive = false;
        }
    }


    public override IEnumerator ExcuteLerp(GameObject reference)
    {

        Vector3 lastPosition = startingPos;
        while (isActive)
        {
            //power of math 
            oscillationTime += Time.deltaTime * frequency;
            float sineValue = Mathf.Sin(oscillationTime * 2f * Mathf.PI);

            Vector3 moveDirection = GetDirection();
            Vector3 targetPos = startingPos + moveDirection * sineValue * amplitude;
            Vector3 velocity = (targetPos - lastPosition) / Time.deltaTime;
            lerpData.SetData(velocity);
            lastPosition = targetPos;
            yield return null;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero; 
        if (oscillation_axis == OSCILLATION_AXIS.X_AXIS)
        {
            if (oscillation_startDirection == START_DIRECTION.POSTIVE)
                direction = Vector2.right;
            else
                direction = Vector2.left; 
        }
        else
        {
            if (oscillation_startDirection == START_DIRECTION.POSTIVE)
                direction = Vector2.up;
            else
                direction = Vector2.down;
        }
        return direction;
    }

    public override void CopyClassData(LerpFunction reference)
    {
        if (reference is LerpBackAndForth lerpBackForth)
        {
            lerpType = reference.lerpType;
            amplitude = lerpBackForth.amplitude;
            frequency = lerpBackForth.frequency;
            oscillation_axis = lerpBackForth.oscillation_axis;
            oscillation_startDirection = lerpBackForth.oscillation_startDirection;
        }
    }
}
