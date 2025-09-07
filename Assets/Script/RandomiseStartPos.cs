using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomiseStartPos", menuName = "Scriptable Objects/RandomiseStartPos")]
public class RandomiseStartPos : LerpFunction
{
    public override IEnumerator ExcuteLerp(GameObject reference)
    {
        Debug.Log("BLANK FOR NOW");
        yield return null;
    }

    public override void Init()
    {
        Debug.Log("Blank");
    }

    public override void Reset()
    {
        Debug.Log("Blank");
    }
}
