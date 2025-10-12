using System.Collections.Generic;
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

    //[HideInInspector] public GameObject currentColliding;
    [HideInInspector] public List<GameObject> allColliding; 


    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (EventCollideByTag target in list)
        {
            bool validCollision = false;
            if (target.tag == collision.gameObject.tag)
                validCollision = true;


            if (validCollision)
            {
                allColliding.Add(collision.gameObject);
                target.onTriggerEnter.Invoke();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        foreach (EventCollideByTag target in list)
        {
            bool validExit = false;
            if (target.tag == collision.gameObject.tag)
                validExit = true;

            if (validExit)
            {
                foreach (GameObject gameObject in allColliding)
                {
                    if (gameObject == collision.gameObject)
                    {
                        target.onTriggerExit.Invoke();
                        allColliding.Remove(gameObject);
                        break;
                    }
                }
                //target.onTriggerExit.Invoke();
            }
        }
    }
}
