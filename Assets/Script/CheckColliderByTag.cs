using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventCollideByTag
{
    public string tag;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
}

public class CheckColliderByTag : MonoBehaviour
{
    //special just for tag checking and hopefully resolves my collision reference quams 
    public EventCollideByTag[] list;

    [HideInInspector] public GameObject currentColliding;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (EventCollideByTag target in list)
        {
            if (target.tag == collision.gameObject.tag)
            {
                //Debug.Log("COLLIDING WITH " + collision.gameObject.name);
                currentColliding = collision.gameObject;
                target.onTriggerEnter.Invoke();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        foreach (EventCollideByTag target in list)
        {
            if (target.tag == collision.gameObject.tag)
            {
                target.onTriggerExit.Invoke();
            }

            if (collision.gameObject == currentColliding)
            {
                currentColliding = null;
            }
        }
    }
}
