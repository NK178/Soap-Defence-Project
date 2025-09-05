using UnityEngine;
using UnityEngine.Events;

public class OnMouseInteracts : MonoBehaviour
{

    [SerializeField] private UnityEvent onMouseEnter = null;
    [SerializeField] private UnityEvent onMouseClick = null;
    [SerializeField] private UnityEvent onMouseExit = null;

    private void OnMouseEnter()
    {
        if (onMouseEnter != null)
        {

        }
    }

    private void OnMouseDown()
    {
        if (onMouseClick != null)
        {

        }
    }

    private void OnMouseExit()
    {
        if (onMouseExit != null)
        {

        }
    }
}
