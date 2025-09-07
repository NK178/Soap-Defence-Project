using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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


    // lerp based on this
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private OSCILLATION_AXIS oscillation_axis; 
    [SerializeField] private START_DIRECTION oscillation_startDirection; 

    private float oscillationTime = 0f;
    private Vector3 centerPos;

    public override void Init(GameObject reference)
    {
        if (!isActive)
        {
            centerPos = reference.transform.position;
            isActive = true;
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
        while (isActive)
        {
            //power of math 
            oscillationTime += Time.deltaTime * frequency;
            float sineValue = Mathf.Sin(oscillationTime * 2f * Mathf.PI);

            Rigidbody2D rb = reference.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                yield return null;
            }
        }
    }

    private Vector3 GetDirection()
    {
        //if (oscillation_axis == OSCILLATION_AXIS.X_AXIS)
        //{
            
        //}
        //if (oscillation_startDirection == START_DIRECTION.POSTIVE)
    }
}
