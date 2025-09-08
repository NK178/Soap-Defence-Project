using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "LerpResponsePhyVelocity", menuName = "Scriptable Objects/LerpResponsePhyVelocity")]
public class LerpResponsePhyVelocity : LerpResponse
{

    private Vector3 totalValue; 

    public override void HandleResponse(GameObject reference)
    {
        Rigidbody2D rb = reference.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = totalValue;
            totalValue = Vector3.zero;
        }
    }

    public override void AddValueToCalculation(ILerpData lerpData)
    {
        if (lerpData is LerpData<Vector3> vector3Lerp)
        {
            totalValue += vector3Lerp.GetTypedData();
        }
    }
}
