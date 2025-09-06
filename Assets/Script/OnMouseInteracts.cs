using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[System.Serializable]
public class MouseEventsByTag
{
    public string tagName;
    public UnityEvent onMouseEnter = null;
    public UnityEvent onMouseClick = null;
    public UnityEvent onMouseExit = null;
    public bool isColliding = false;

}

public class OnMouseInteracts : MonoBehaviour
{
    //idk if this is good or bad design but I will roll with it for now 
    [SerializeField] protected MousePositionReference mouseInstance;
    [SerializeField] private string leftClickName;
    [SerializeField] private List<MouseEventsByTag> responseList;
    InputAction leftClick;

    private void OnEnable()
    {
        leftClick = InputSystem.actions.FindAction(leftClickName);
        if (leftClick != null)
        {
            leftClick.performed += HandleMouseClick;
            leftClick.Enable();
        }
    }

    private void OnDisable()
    {
        leftClick.performed -= HandleMouseClick;
    }

    public void HandleMouseEnterExit()
    {
        for (int iter = 0; iter < responseList.Count; iter++)
        {

            if (IsMouseColliding(responseList[iter]))
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
        }
    }

    public void HandleMouseClick(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            for (int iter = 0; iter < responseList.Count; iter++)
            {
                if (IsMouseColliding(responseList[iter]) && responseList[iter].onMouseClick != null)
                {
                    responseList[iter].onMouseClick.Invoke();
                    break;
                }
            }
        }
    }

    private void Update()
    {
        HandleMouseEnterExit();
    }

    bool IsMouseColliding(MouseEventsByTag eventTag)
    {
        bool validCollide = false;
        Vector2 worldMousePos = mouseInstance.GetWorldMousePos();
        Collider2D hit = Physics2D.OverlapPoint(worldMousePos);
        if (hit == GetComponent<Collider2D>() && hit.gameObject.CompareTag(eventTag.tagName))
        {
            Debug.Log("Collider2D is being hit! " + hit.gameObject.name);
            validCollide = true;
        }
        return validCollide;
    }
}

