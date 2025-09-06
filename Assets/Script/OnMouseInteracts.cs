using UnityEngine;
using UnityEngine.Events;

public abstract class OnMouseInteracts : MonoBehaviour
{

    public abstract void OnMouseEnter();
    public abstract void OnMouseDown();
    public abstract void OnMouseExit();
}



//// issue with this is that it doesnt work on prefabs so maybe need to do abstract class strat
//public class OnMouseInteracts : MonoBehaviour
//{

//    [SerializeField] private UnityEvent onMouseEnter = null;
//    [SerializeField] private UnityEvent onMouseClick = null;
//    [SerializeField] private UnityEvent onMouseExit = null;

//    private void OnMouseEnter()
//    {
//        if (onMouseEnter != null)
//        {

//        }
//    }

//    private void OnMouseDown()
//    {
//        if (onMouseClick != null)
//        {

//        }
//    }

//    private void OnMouseExit()
//    {
//        if (onMouseExit != null)
//        {

//        }
//    }
//}
