
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SoapBarProjectileAttack", menuName = "Scriptable Objects/SoapBarProjectileAttack")]
public class SoapBarProjectileAttack : EntityFunctions
{


    [SerializeField] private GameObject soapChipPrefab;
    [SerializeField] private float attackRate;
    [SerializeField] private float fixedProjectileSpeed;
    [SerializeField] private float gravityScale; 

    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {
        Vector3 baseDistance = Vector3.zero;
        //run infintely until stopped 
        while (true)
        {
            Vector3 spawnPosition = entity.gameObject.transform.position;

            //raycast to find nearest enemy 
            bool isEnemyFound = false;
            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.right, 75f, LayerMask.GetMask("enemy"));
            Vector3 endPosition = spawnPosition + Vector3.right * 75f;
            Debug.DrawRay(spawnPosition, endPosition, Color.red);
            Entity target = null;
            if (hit.collider != null)
            {
                target = hit.collider.gameObject.GetComponent<Entity>();
                if (target != null)
                    isEnemyFound = true;
            }

            //THE ONE THAT ACTUALLY WORKS 
            if (isEnemyFound && target != null)
            {
                //calculate trajectory which will be hard bruh cyka
                GameObject objectReference = Instantiate(soapChipPrefab, spawnPosition, entity.gameObject.transform.rotation);
                ProjectileBallistics projectile = objectReference.GetComponent<ProjectileBallistics>();

                //bad system probably needs to change but will do for now 
                RequireParentReference typeData = projectile.GetComponent<RequireParentReference>();
                if (typeData != null)
                {
                    typeData.SetReferenceEntity(entity);
                    typeData.SetTargetEntity(target);
                }

                    float projectileSpeed = fixedProjectileSpeed;
                Vector3 targetPosition = target.transform.position - spawnPosition;
                Vector3 targetSpeed = target.dataLibrary.GetVector3("MoveInDirection_velocity");
                Vector3 projectileGravity = gravityScale * Vector2.down;
                Vector3 fireVelocity = Vector3.zero;
                bool shouldFire = solve_ballistic_with_fixed_speed(targetPosition, targetSpeed, projectileGravity, projectileSpeed, out fireVelocity);
                if (shouldFire)
                {
                    entity.PlayAnimation(ENTITYANIMS.ATTACK);
                    projectile.Initialize(spawnPosition, gravityScale);
                    projectile.AddImpulse(fireVelocity);
                    Debug.DrawRay(spawnPosition, fireVelocity, Color.red, 2f);
                    Debug.DrawLine(spawnPosition, targetPosition + spawnPosition, Color.green, 2f);
                }
            }
            yield return new WaitForSeconds(attackRate);
        }
    }

    public static bool solve_ballistic_with_fixed_speed(Vector3 targetPos, Vector3 targetVel, Vector3 gravity, float projSpeed, out Vector3 fire_velocity)
    {
        fire_velocity = Vector3.zero;
        double c0 = 0.25 * Vector3.Dot(gravity, gravity);
        double c1 = Vector3.Dot(gravity, targetVel);
        double c2 = Vector3.Dot(targetPos, gravity) + Vector3.Dot(targetVel, targetVel) - projSpeed * projSpeed;
        double c3 = 2 * Vector3.Dot(targetPos, targetVel);
        double c4 = Vector3.Dot(targetPos, targetPos);
        double[] solutions = new double[4];
        int numTimes = MathFile.SolveQuartic(c0, c1, c2, c3, c4, out solutions[0], out solutions[1], out solutions[2], out solutions[3]);
        float t = float.MaxValue;

        if (numTimes == 0)
        {
            Debug.Log("No valid time solutions found");
            return false;
        }

        // Find the smallest positive time solution
        bool foundValidTime = false;
        float smallest = float.MaxValue;
        float secondSmallest = float.MaxValue;

        for (int i = 0; i < numTimes; i++)
        {
            if (solutions[i] > 0)
            {
                if (solutions[i] < smallest)
                {
                    secondSmallest = smallest;
                    smallest = (float)solutions[i];
                }
                else if (solutions[i] < secondSmallest)
                {
                    secondSmallest = (float)solutions[i];
                }
            }
        }
        t = (secondSmallest != float.MaxValue) ? secondSmallest : smallest;
        if (t > 0)
            foundValidTime = true;



        if (!foundValidTime)
        {
            Debug.Log("No positive time solutions found");
            return false;
        }


        // Calculate where the target will be at time t
        Vector3 futureTargetPos = targetPos + t * targetVel;

        // Calculate the required initial velocity to hit that position
        // Using kinematic equation: futurePos = initialVel * t + 0.5 * gravity * t^2
        // Solving for initialVel: initialVel = (futurePos - 0.5 * gravity * t^2) / t
        fire_velocity = (futureTargetPos - 0.5f * gravity * t * t) / t;
        return true;
    }
}
