using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;


[System.Serializable]
public class MouseEventsByTag
{
    public string tagName;
    public UnityEvent onMouseEnter = null;
    public UnityEvent onMouseClick = null;
    public UnityEvent onMouseRelease = null;
    public UnityEvent onMouseExit = null;
    [HideInInspector] public bool isColliding = false;
}

public class OnMouseInteracts : MonoBehaviour
{
    //idk if this is good or bad design but I will roll with it for now 
    [SerializeField] protected MousePositionReference mouseInstance;
    [SerializeField] private string nameLeftClickPress;
    [SerializeField] private string nameLeftClickRelease;
    [SerializeField] private List<MouseEventsByTag> responseList;
    [HideInInspector] public GameObject currentTarget;
    InputAction leftClick;
    InputAction leftRelease;

    private void OnEnable()
    {
        leftClick = InputSystem.actions.FindAction(nameLeftClickPress);
        leftRelease = InputSystem.actions.FindAction(nameLeftClickRelease);
        if (leftClick != null)
        {
            leftClick.performed += HandleMousePress;
            leftClick.Enable();
        }
        if (leftRelease != null)
        {
            leftRelease.performed += HandleMouseRelease;
            leftRelease.Enable();
        }
    }

    private void OnDisable()
    {
        leftClick.performed -= HandleMousePress;
        leftRelease.performed -= HandleMouseRelease;
    }

    public void HandleMouseEnterExit()
    {
        //let this be useless first 
        GameObject referenceObject = null;

        for (int iter = 0; iter < responseList.Count; iter++)
        {

            if (IsMouseCollidingValid(responseList[iter], ref referenceObject))
            {
                if (responseList[iter].onMouseEnter != null)
                {
                    responseList[iter].onMouseEnter.Invoke();
                    responseList[iter].isColliding = true;
                }
            }
            else
            {
                //dont keep repeating the exit invoke (so invoke only if is collidng is true)
                if (responseList[iter].onMouseExit != null && responseList[iter].isColliding)
                {
                    responseList[iter].onMouseEnter.Invoke();
                    responseList[iter].isColliding = false;
                }
            }


            //set the gameobejct 
            if (referenceObject != null)
                currentTarget = referenceObject;

        }
    }

    public void HandleMousePress(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            for (int iter = 0; iter < responseList.Count; iter++)
            {
                GameObject notUsed = null;
                if (IsMouseCollidingValid(responseList[iter], ref notUsed) && responseList[iter].onMouseClick != null)
                {
                    responseList[iter].onMouseClick.Invoke();
                    break;
                }
            }

            TESTING = true;
        }
    }


    public void HandleMouseRelease(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            for (int iter = 0; iter < responseList.Count; iter++)
            {
                GameObject notUsed = null;
                if (IsMouseCollidingValid(responseList[iter], ref notUsed) && responseList[iter].onMouseRelease != null)
                {
                    responseList[iter].onMouseRelease.Invoke();
                    break;
                }
            }
        }
    }

    bool TESTING = false;
    private void Update()
    {
        HandleMouseEnterExit();


        if (TESTING)
        {

            //TESTING 
            Vector3 spawnPosition = new Vector3(400, 300, 0);

            // Clear any existing debug lines first
            Debug.ClearDeveloperConsole();

            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.right, 20f, LayerMask.GetMask("enemy"));

            Vector3 endPosition = spawnPosition + Vector3.right * 200f;
            Debug.DrawLine(spawnPosition, endPosition, Color.red, 2f); // 2 second duration

            Debug.Log($"SPAWN: {spawnPosition}");
            Debug.Log($"END: {endPosition}");

            if (hit.collider != null)
            {
                Debug.Log("ENEMY HIT: " + hit.collider.name);
                Debug.DrawLine(spawnPosition, hit.point, Color.green, 2f);
            }
        }
    }

        bool IsMouseCollidingValid(MouseEventsByTag eventTag, ref GameObject referenceObject)
    {
        bool validCollide = false;
        Vector2 worldMousePos = mouseInstance.GetWorldMousePos();
        Collider2D hit = Physics2D.OverlapPoint(worldMousePos);
        if (hit != null && hit.gameObject.CompareTag(eventTag.tagName))
        {
            //Debug.Log("Collider2D is being hit! " + hit.gameObject.name);
            validCollide = true;
            referenceObject = hit.gameObject;
        }
        else
            referenceObject = null;
        return validCollide;
    }


    //maybe can use in the future
    ////THIS FUNCTION HAS NO CHECKS WHETHER IS VAILD 
    //GameObject GetColliderUnderMouse()
    //{
    //    Vector2 worldMousePos = mouseInstance.GetWorldMousePos();
    //    Collider2D hit = Physics2D.OverlapPoint(worldMousePos);
    //    if (hit)
    //        return GameObject
    //}
}

