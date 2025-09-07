using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LerpBackAndForth", menuName = "Scriptable Objects/LerpBackAndForth")]
public class LerpBackAndForth : LerpFunction
{
    public override IEnumerator ExcuteLerp(GameObject reference)
    {
        Debug.Log("Blank");
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
