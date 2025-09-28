using Mono.Cecil;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;
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

            ////testing fixed speed version 
            //if (isEnemyFound)
            //{
            //    //calculate trajectory which will be hard bruh cyka
            //    GameObject objectReference = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);
            //    ProjectileBallistics projectile = objectReference.GetComponent<ProjectileBallistics>();

            //    //bad system probably needs to change but will do for now 
            //    RequireParentReference typeData = projectile.GetComponent<RequireParentReference>();
            //    if (typeData != null)
            //        typeData.SetReferenceEntity(parentObject.GetComponent<Entity>());

            //    float projectileSpeed = 15f;
            //    Vector3 targetPosition = projectile.transform.position - parentObject.transform.position;
            //    Vector3 targetSpeed = 5f * Vector2.right;
            //    Vector3 projectileGravity = 9.81f * Vector2.down; 
            //    Vector3 fireVelocity = Vector3.zero;

            //    solve_ballistic_with_fixed_speed(targetPosition, targetSpeed, projectileGravity, projectileSpeed, out fireVelocity);
            //    projectile.Initialize(projectile.transform.position, 9.81f);
            //    projectile.AddImpulse(fireVelocity);
            //    Debug.Log(fireVelocity);
            //}

            //this one works, lateral speed version 
            if (isEnemyFound)
            {
                //calculate trajectory which will be hard bruh cyka
                GameObject objectReference = Instantiate(soapChipPrefab, spawnPosition, parentObject.transform.rotation);
                ProjectileBallistics projectile = objectReference.GetComponent<ProjectileBallistics>();

                //bad system probably needs to change but will do for now 
                RequireParentReference typeData = projectile.GetComponent<RequireParentReference>();
                if (typeData != null)
                    typeData.SetReferenceEntity(parentObject.GetComponent<Entity>());

                float lateralSpeed = 13f;
                float maxHeight = 8f;
                Vector3 fireVelocity = Vector3.zero;
                float projGravity = 0f;
                solve_ballistic_arc_lateral(projectile.transform.position, lateralSpeed, hit.collider.gameObject.transform.position,
                    maxHeight, out fireVelocity, out projGravity);
                projectile.Initialize(projectile.transform.position, projGravity);
                projectile.AddImpulse(fireVelocity);
            }

            //this one works, actual speed version 
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


    //testing out fixed speed method see if this works 
    //hmmmmmmmm
    public static bool solve_ballistic_with_fixed_speed(Vector3 targetPos, Vector3 targetVel, Vector3 gravity, float projSpeed, out Vector3 fire_velocity)
    {
        fire_velocity = Vector3.zero;
        double c0 = 0.25 * Vector3.Dot(gravity, gravity);
        double c1 = Vector3.Dot(gravity, targetVel);
        double c2 = Vector3.Dot(targetPos, gravity) + Vector3.Dot(targetVel, targetVel) - projSpeed * projSpeed;
        double c3 = 2 * Vector3.Dot(targetPos, targetVel);
        double c4 = Vector3.Dot(targetPos, targetPos);
        double[] solutions = new double[4];
        int numTimes = fts.SolveQuartic(c0, c1, c2, c3,  c4, out solutions[0], out solutions[1], out solutions[2], out solutions[3]);
        float t = float.MaxValue;

        if (numTimes == 0)
        {
            Debug.Log("No valid time solutions found");
            return false;
        }

        // Find the smallest positive time solution
        bool foundValidTime = false;

        for (int i = 0; i < numTimes; i++)
        {
            if (solutions[i] > 0 && solutions[i] < t)
            {
                t = (float)solutions[i];
                foundValidTime = true;
            }
        }

        if (!foundValidTime)
        {
            Debug.Log("No positive time solutions found");
            return false;
        }

        Debug.Log("Using T value: " + t);

        // Calculate where the target will be at time t
        Vector3 futureTargetPos = targetPos + t * targetVel;

        // Calculate the required initial velocity to hit that position
        // Using kinematic equation: futurePos = initialVel * t + 0.5 * gravity * t^2
        // Solving for initialVel: initialVel = (futurePos - 0.5 * gravity * t^2) / t
        fire_velocity = (futureTargetPos - 0.5f * gravity * t * t) / t;

        // Verify the speed constraint
        float actualSpeed = fire_velocity.magnitude;
        if (Mathf.Abs(actualSpeed - projSpeed) > 0.001f)
        {
            Debug.LogWarning($"Speed constraint violated: expected {projSpeed}, got {actualSpeed}");
            // Normalize to correct speed if needed
            fire_velocity = fire_velocity.normalized * projSpeed;
        }

        return true;


        //if (numTimes != 0)
        //    t = (float)solutions[2];

        //Debug.Log("T value " + t);
        //Vector3 finalVel = targetPos + t * targetVel;
        //fire_velocity = finalVel;

        //return true;
    }


    //kay this one works 
    //quite good for artilery shells I think just not in this case 
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


//////////////////////////////////// TEMP BUT FOR NOW THIS IS HERE /////////////////////////
public class fts
{

    // SolveQuadric, SolveCubic, and SolveQuartic were ported from C as written for Graphics Gems I
    // Original Author: Jochen Schwarze (schwarze@isa.de)
    // https://github.com/erich666/GraphicsGems/blob/240a34f2ad3fa577ef57be74920db6c4b00605e4/gems/Roots3And4.c

    // Utility function used by SolveQuadratic, SolveCubic, and SolveQuartic
    private static bool IsZero(double d)
    {
        const double eps = 1e-9;
        return d > -eps && d < eps;
    }

    private static double GetCubicRoot(double value)
    {
        if (value > 0.0)
        {
            return System.Math.Pow(value, 1.0 / 3.0);
        }
        else if (value < 0)
        {
            return -System.Math.Pow(-value, 1.0 / 3.0);
        }
        else
        {
            return 0.0;
        }
    }

    // Solve quadratic equation: c0*x^2 + c1*x + c2. 
    // Returns number of solutions.
    public static int SolveQuadric(double c0, double c1, double c2, out double s0, out double s1)
    {
        s0 = double.NaN;
        s1 = double.NaN;

        double p, q, D;

        /* normal form: x^2 + px + q = 0 */
        p = c1 / (2 * c0);
        q = c2 / c0;

        D = p * p - q;

        if (IsZero(D))
        {
            s0 = -p;
            return 1;
        }
        else if (D < 0)
        {
            return 0;
        }
        else /* if (D > 0) */
        {
            double sqrt_D = System.Math.Sqrt(D);

            s0 = sqrt_D - p;
            s1 = -sqrt_D - p;
            return 2;
        }
    }

    // Solve cubic equation: c0*x^3 + c1*x^2 + c2*x + c3. 
    // Returns number of solutions.
    public static int SolveCubic(double c0, double c1, double c2, double c3, out double s0, out double s1, out double s2)
    {
        s0 = double.NaN;
        s1 = double.NaN;
        s2 = double.NaN;

        int num;
        double sub;
        double A, B, C;
        double sq_A, p, q;
        double cb_p, D;

        /* normal form: x^3 + Ax^2 + Bx + C = 0 */
        A = c1 / c0;
        B = c2 / c0;
        C = c3 / c0;

        /*  substitute x = y - A/3 to eliminate quadric term:  x^3 +px + q = 0 */
        sq_A = A * A;
        p = 1.0 / 3 * (-1.0 / 3 * sq_A + B);
        q = 1.0 / 2 * (2.0 / 27 * A * sq_A - 1.0 / 3 * A * B + C);

        /* use Cardano's formula */
        cb_p = p * p * p;
        D = q * q + cb_p;

        if (IsZero(D))
        {
            if (IsZero(q)) /* one triple solution */
            {
                s0 = 0;
                num = 1;
            }
            else /* one single and one double solution */
            {
                double u = GetCubicRoot(-q);
                s0 = 2 * u;
                s1 = -u;
                num = 2;
            }
        }
        else if (D < 0) /* Casus irreducibilis: three real solutions */
        {
            double phi = 1.0 / 3 * System.Math.Acos(-q / System.Math.Sqrt(-cb_p));
            double t = 2 * System.Math.Sqrt(-p);

            s0 = t * System.Math.Cos(phi);
            s1 = -t * System.Math.Cos(phi + System.Math.PI / 3);
            s2 = -t * System.Math.Cos(phi - System.Math.PI / 3);
            num = 3;
        }
        else /* one real solution */
        {
            double sqrt_D = System.Math.Sqrt(D);
            double u = GetCubicRoot(sqrt_D - q);
            double v = -GetCubicRoot(sqrt_D + q);

            s0 = u + v;
            num = 1;
        }

        /* resubstitute */
        sub = 1.0 / 3 * A;

        if (num > 0) s0 -= sub;
        if (num > 1) s1 -= sub;
        if (num > 2) s2 -= sub;

        return num;
    }

    // Solve quartic function: c0*x^4 + c1*x^3 + c2*x^2 + c3*x + c4. 
    // Returns number of solutions.
    public static int SolveQuartic(double c0, double c1, double c2, double c3, double c4, out double s0, out double s1, out double s2, out double s3)
    {
        s0 = double.NaN;
        s1 = double.NaN;
        s2 = double.NaN;
        s3 = double.NaN;

        double[] coeffs = new double[4];
        double z, u, v, sub;
        double A, B, C, D;
        double sq_A, p, q, r;
        int num;

        /* normal form: x^4 + Ax^3 + Bx^2 + Cx + D = 0 */
        A = c1 / c0;
        B = c2 / c0;
        C = c3 / c0;
        D = c4 / c0;

        /*  substitute x = y - A/4 to eliminate cubic term: x^4 + px^2 + qx + r = 0 */
        sq_A = A * A;
        p = -3.0 / 8 * sq_A + B;
        q = 1.0 / 8 * sq_A * A - 1.0 / 2 * A * B + C;
        r = -3.0 / 256 * sq_A * sq_A + 1.0 / 16 * sq_A * B - 1.0 / 4 * A * C + D;

        if (IsZero(r))
        {
            /* no absolute term: y(y^3 + py + q) = 0 */

            coeffs[3] = q;
            coeffs[2] = p;
            coeffs[1] = 0;
            coeffs[0] = 1;

            num = fts.SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);
        }
        else
        {
            /* solve the resolvent cubic ... */
            coeffs[3] = 1.0 / 2 * r * p - 1.0 / 8 * q * q;
            coeffs[2] = -r;
            coeffs[1] = -1.0 / 2 * p;
            coeffs[0] = 1;

            SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);

            /* ... and take the one real solution ... */
            z = s0;

            /* ... to build two quadric equations */
            u = z * z - r;
            v = 2 * z - p;

            if (IsZero(u))
                u = 0;
            else if (u > 0)
                u = System.Math.Sqrt(u);
            else
                return 0;

            if (IsZero(v))
                v = 0;
            else if (v > 0)
                v = System.Math.Sqrt(v);
            else
                return 0;

            coeffs[2] = z - u;
            coeffs[1] = q < 0 ? -v : v;
            coeffs[0] = 1;

            num = fts.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);

            coeffs[2] = z + u;
            coeffs[1] = q < 0 ? v : -v;
            coeffs[0] = 1;

            if (num == 0) num += fts.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);
            else if (num == 1) num += fts.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s1, out s2);
            else if (num == 2) num += fts.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s2, out s3);
        }

        /* resubstitute */
        sub = 1.0 / 4 * A;

        if (num > 0) s0 -= sub;
        if (num > 1) s1 -= sub;
        if (num > 2) s2 -= sub;
        if (num > 3) s3 -= sub;

        return num;
    }
}