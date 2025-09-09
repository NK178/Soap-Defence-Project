using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SoapBarProjectileAttack", menuName = "Scriptable Objects/SoapBarProjectileAttack")]
public class SoapBarProjectileAttack : EntityFunctions
{


    [SerializeField] private GameObject soapChipPrefab;
    [SerializeField] private float attackRate; 
    
    public override IEnumerator ExcuteCoroutine(GameObject parentObject = null)
    {
        if (parentObject == null)
            yield return null;

        //run infintely until stopped 
        while (true)
        {
            Vector3 spawnPosition = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);

            //calculate trajectory which will be hard bruh cyka
            GameObject newBubble = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);

            yield return new WaitForSeconds(attackRate);
        }
    }
}
