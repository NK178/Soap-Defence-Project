using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
            //Vector3 spawnPosition = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);
            ////raycast to find nearest enemy 
            //RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.right,40f, LayerMask.GetMask("enemy"));
            //Vector3 endPosition = spawnPosition + Vector3.right * 20f;
            //Debug.DrawRay(spawnPosition, endPosition, Color.red);

            //Debug.Log("SPAWN " + spawnPosition);
            //Debug.Log("END " + endPosition);
            //if (hit.collider != null)
            //{
            //    Debug.Log("ENEMY HIT");
            //}
            //else
            //    Debug.Log("CANT FIND");
            ////calculate trajectory which will be hard bruh cyka
            //GameObject projectile = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);


            ////testing 
            //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //rb.AddForce(Vector2.right * 50,ForceMode2D.Impulse);
            //yield return new WaitForSeconds(attackRate);
            yield return null;
        }
    }
}
