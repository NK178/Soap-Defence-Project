using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    //private InputAction OnLeftMouse;
    //private InputAction OnRightMouse;

    private void OnEnable()
    {
        //4/9 for the dragging system i dont need thsi find action response because I will be using drag drop handler to handle this 
        // but for the clicking part I probably need this 
        // unless I use the click handler i guess 
        //OnLeftMouse = InputSystem.actions.FindAction("LeftMouse");
        //OnRightMouse = InputSystem.actions.FindAction("RightMouse");

    }



    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }



    

    // Update is called once per frame
    void Update()
    {
        if (ShopManager.instance.isDragging)
        {
            ShopManager.instance.GetCurrentItem();
        }
    }
}
