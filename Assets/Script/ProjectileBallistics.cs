using UnityEngine;

public class ProjectileBallistics : MonoBehaviour
{
    // Private fields
    Vector3 lastPos;
    Vector3 impulse;
    float gravity;

    public void Initialize(Vector3 pos, float gravity)
    {
        transform.position = pos;
        lastPos = transform.position;
        this.gravity = gravity;
    }

    void FixedUpdate()
    {
        // Simple verlet integration
        float dt = Time.fixedDeltaTime;
        Vector3 accel = -gravity * Vector3.up;

        Vector3 curPos = transform.position;
        Vector3 newPos = curPos + (curPos - lastPos) + impulse * dt + accel * dt * dt;
        lastPos = curPos;
        transform.position = newPos;
        impulse = Vector3.zero;
    }

    public void AddImpulse(Vector3 impulse)
    {
        this.impulse += impulse;
    }
}

