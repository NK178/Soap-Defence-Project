using Mono.Cecil;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Compilation;
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

            //Testing version 
            //I think need to handle the velocity by itself which is kinda annoying
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

                float lateralSpeed = 10f;
                float maxHeight = 3f;
                Vector3 fireVelocity = Vector3.zero;
                float projGravity = 0f;
                solve_ballistic_arc_lateral(projectile.transform.position, lateralSpeed, hit.collider.gameObject.transform.position,
                    maxHeight, out fireVelocity, out projGravity);
                rb.gravityScale = projGravity;
                rb.linearVelocity = fireVelocity;

            }


            //working version that isnt using lateral speed 
            //if (isEnemyFound)
            //{
            //    //calculate trajectory which will be hard bruh cyka
            //    GameObject projectile = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);


            //    //testing 
            //    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //    //rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse);

            //    //bad system probably needs to change but will do for now 
            //    RequireParentReference typeData = projectile.GetComponent<RequireParentReference>();
            //    if (typeData != null)
            //        typeData.SetReferenceEntity(parentObject.GetComponent<Entity>());
            //    rb.linearVelocity = CalculateProjectileVelocity(parentObject, hit.collider.gameObject);
            //}

            yield return new WaitForSeconds(attackRate);
        }
    }



    //I dont understand this what the hell 
    public static bool solve_ballistic_arc_lateral(Vector3 proj_pos, float lateral_speed, Vector3 target_pos, float max_height, out Vector3 fire_velocity, out float gravity)
    {

        // Handling these cases is up to your project's coding standards
        Debug.Assert(proj_pos != target_pos && lateral_speed > 0 && max_height > proj_pos.y, "fts.solve_ballistic_arc called with invalid data");

        fire_velocity = Vector3.zero;
        gravity = float.NaN;

        Vector3 diff = target_pos - proj_pos;
        Vector3 diffXZ = new Vector3(diff.x, 0f, diff.z);
        float lateralDist = diffXZ.magnitude;

        if (lateralDist == 0)
            return false;

        float time = lateralDist / lateral_speed;

        fire_velocity = diffXZ.normalized * lateral_speed;

        // System of equations. Hit max_height at t=.5*time. Hit target at t=time.
        //
        // peak = y0 + vertical_speed*halfTime + .5*gravity*halfTime^2
        // end = y0 + vertical_speed*time + .5*gravity*time^s
        // Wolfram Alpha: solve b = a + .5*v*t + .5*g*(.5*t)^2, c = a + vt + .5*g*t^2 for g, v
        float a = proj_pos.y;       // initial
        float b = max_height;       // peak
        float c = target_pos.y;     // final

        gravity = -4 * (a - 2 * b + c) / (time * time);
        fire_velocity.y = -(3 * a - 4 * b + c) / time;

        return true;
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
        //Debug.Log("FINAL VECTOR " + finalVector);
        return finalVector;
    }
}
