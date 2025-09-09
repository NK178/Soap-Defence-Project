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
                rb.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
                CalculateProjectileVelocity(parentObject, hit.collider.gameObject);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }


    // 9/9 idk how to do this bruh 
    // need to go study 
    //problemo is that I need a "fixed speed" , but I need to have angle and lateralSpeed to change 
    // need fixed speed to ensure that the projectile will land at the same time no matter where the enemy is 
    public void CalculateProjectileVelocity(GameObject self, GameObject targetObj)
    {
        ////testing cos idk how to do yet 
        //float lateralSpeed = 10f;
        //float max_height = 40f;

        //Vector3 displacement = targetObj.transform.position - self.transform.position;
        //float lateralDistance = displacement.magnitude;

        //float time = lateralDistance / lateralDistance;
        //Vector3 fire_velocity = displacement.normalized * lateralSpeed;
        //float a = self.transform.position.y;       // initial
        //float b = max_height;       // peak
        //float c = targetObj.transform.position.y;     // final

        //float gravity = -4 * (a - 2 * b + c) / (time * time);
        //fire_velocity.y = -(3 * a - 4 * b + c) / time;

        //Debug.Log(gravity);

        //Vector2 displacement = targetObj.transform.position - self.transform.position;
        //float horizontalDistance = displacement.x;
        //float verticalDistance = displacement.y;
        //float FIXED_TIME = 1.0f;
        //float FIXED_ANGLE = 30f;
        //float gravity = Mathf.Abs(Physics2D.gravity.y);

        //float horizontalVelocity = horizontalDistance / FIXED_TIME;

        // Using y = y0 + velocity_y0 * t  - 1/2 * g * t^2 
        // y0 = 0 , need to find initial velocity of y0

        //using da power of pythagoras 
        //float intialVelocity = 
        //float verticalVelocity = 

        //// Calculate the actual launch angle
        //float launchAngle = Mathf.Atan2(verticalVelocity, horizontalVelocity) * Mathf.Rad2Deg;

        //// Calculate initial speed (this will be lower for higher arcs)
        //float initialSpeed = Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity);

        //// Calculate peak height of the trajectory
        //float peakHeight = (verticalVelocity * verticalVelocity) / (2 * gravity);

        //// Calculate when peak occurs
        //float timeToApex = verticalVelocity / gravity;

        //Debug.Log($"Launch Angle: {launchAngle}°");
        //Debug.Log($"Initial Speed: {initialSpeed}");
        //Debug.Log($"Peak Height: {peakHeight}");
        //Debug.Log($"Time to Apex: {timeToApex}");
        //Debug.Log($"Velocity: ({horizontalVelocity}, {verticalVelocity})");

        //Vector2 displacement = targetObj.transform.position - self.transform.position;
        //float horizontalDistance = displacement.x;
        //float verticalDistance = displacement.y;

        //float time = 1.0f; // Fixed flight time

        //// Calculate required horizontal velocity for fixed time
        //float horizontalSpeed = horizontalDistance / time;

        //// Calculate required vertical velocity component
        //float gravity = Mathf.Abs(Physics2D.gravity.y);
        //float initialVelocityY = (verticalDistance + 0.5f * gravity * time * time) / time;

        //// Calculate actual launch angle needed
        //float actualAngle = Mathf.Atan2(initialVelocityY, horizontalSpeed) * Mathf.Rad2Deg;

        //// Peak height calculation (correct formula)
        //float peakHeight = (initialVelocityY * initialVelocityY) / (2 * gravity);

        //Debug.Log($"Required angle: {actualAngle}°, Peak height: {peakHeight}");

        ////assume time is always in a ratio of 1 
        //float distance = (targetObj.transform.position - self.transform.position).magnitude;
        //float time = 1.0f;
        //float lateralSpeed = distance / time;
        //float gravityY = Mathf.Abs(Physics2D.gravity.y);
        //float TEST_ANGLE = 30f;
        ///*
        //find the peak where Y velocity = 0 ,  
        //using velocity_y ^ 2  = velocity_y0 ^ 2 - 2g(y - y0)
        //at peak , current velocity(velocity_y) = y0 = 0 
        //thus equation becomes y = velocity_y0 ^ 2 / 2g 
        //*/

        //float initialVelocityY = lateralSpeed * Mathf.Sin(Mathf.Deg2Rad * TEST_ANGLE);
        //float peakHeight = (initialVelocityY * initialVelocityY) / (2 * gravityY);
        //Debug.Log("PROJECTILE PEAK HEIGHT" + peakHeight);
    }
}
