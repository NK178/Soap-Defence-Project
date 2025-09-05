using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    //private InputAction OnLeftMouse;
    //private InputAction OnRightMouse;
    [SerializeField] private GameObject mouseImage;
    private Mouse mouse;
    private Camera cam;


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
        mouse = Mouse.current;
        cam = Camera.main;
        mouseImage.SetActive(false);
    }





    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = mouse.position.ReadValue();
        Vector3 worldMousePos = new Vector3(cam.ScreenToWorldPoint(mousePos).x, cam.ScreenToWorldPoint(mousePos).y, 0);

        if (ShopManager.instance.isDragging)
        {
            ShopItem currentItem = ShopManager.instance.GetCurrentItem();
            SpriteRenderer sR = mouseImage.GetComponent<SpriteRenderer>();
            if (currentItem != null && sR != null)
            {
                sR.sprite = currentItem.GetSprite();
            }
            mouseImage.SetActive(true);
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else
        {
            mouseImage.SetActive(false);
        }
    }
}
