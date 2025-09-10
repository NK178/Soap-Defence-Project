using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "SoapBarProjectileAttack", menuName = "Scriptable Objects/SoapBarProjectileAttack")]
public class SoapBarProjectileAttack : EntityFunctions
{


    [SerializeField] private GameObject soapChipPrefab;
    [SerializeField] private float attackRate; 

    private float fixedSpeed = 10f;
    
    
    public override IEnumerator ExcuteCoroutine(GameObject parentObject = null)
    {
        if (parentObject == null)
            yield return null;

        //run infintely until stopped 
        while (true)
        {
            Vector3 spawnPosition = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z);

            //raycast to find nearest enemy 
            bool isEnemyFound = false;
            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.right, 75f, LayerMask.GetMask("enemy"));
            Vector3 endPosition = spawnPosition + Vector3.right * 75f;
            Debug.DrawRay(spawnPosition, endPosition, Color.red);
            if (hit.collider != null)
                isEnemyFound = true;


            
            if (isEnemyFound)
            {
                //calculate trajectory which will be hard bruh cyka
                GameObject projectile = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);


                //testing 
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                //rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse);

                //bad system probably needs to change but will do for now 
                RequireParentReference typeData = projectile.GetComponent<RequireParentReference>();
                if (typeData != null)
                    typeData.SetReferenceEntity(parentObject.GetComponent<Entity>());
                rb.linearVelocity = CalculateProjectileVelocity(parentObject, hit.collider.gameObject);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }

    //this works, lets see if it can be better 
    public Vector3 CalculateProjectileVelocity(GameObject self, GameObject targetObj)
    {
        Vector3 finalVector = Vector3.zero;
        float S = 50f;
        float G = -Physics2D.gravity.y * 5;

        Vector3 displacement = targetObj.transform.position - self.transform.position;
        float groundDist = displacement.magnitude;
        float speed2 = S * S;
        float speed4 = S * S * S * S;
        float y = displacement.y;
        float x = groundDist;
        float gx = G * x;
        float root = speed4 - G * (G * x * x + 2 * y * speed2);
        root = Mathf.Sqrt(root);

        float lowAng = Mathf.Atan2(speed2 - root, gx);
        float highAng = Mathf.Atan2(speed2 + root, gx);

        finalVector = displacement.normalized * Mathf.Cos(highAng) * S + Vector3.up * Mathf.Sin(highAng) * S;
        Debug.Log("FINAL VECTOR " + finalVector);
        return finalVector; 
    }
}
