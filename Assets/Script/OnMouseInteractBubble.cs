using UnityEngine;
using UnityEngine.InputSystem;

public class OnMouseInteractBubble : OnMouseInteracts
{

    [SerializeField] private float amountToAdd;
    public override void OnMouseDown()
    {
        Debug.Log("Collected Bubble");
        if (ShopManager.instance != null)
        {
            ShopManager.instance.AddMoney(amountToAdd);
            Destroy(gameObject);
        }
    }

    public override void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");
    }

    public override void OnMouseExit()
    {
        Debug.Log("Mouse Exited");

    }

    InputAction mouseClick;
    Mouse mouse;
    Camera cam;
    private void Start()
    {
        mouseClick = InputSystem.actions.FindAction("LeftMouse");
        mouseClick.Enable();
        mouseClick.performed += DebugClick;
        mouse = Mouse.current;
        cam = Camera.main;
    }

    void Update()
    {


    }


    void DebugClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Clicking");
            Vector2 mousePos = mouse.position.ReadValue();
            Vector3 worldMousePos = new Vector3(cam.ScreenToWorldPoint(mousePos).x, cam.ScreenToWorldPoint(mousePos).y, 0);
            Collider2D hit = Physics2D.OverlapPoint(worldMousePos);
            if (hit == GetComponent<Collider2D>())
            {
                Debug.Log("Collider2D is being hit! " + hit.gameObject.name);
                if (ShopManager.instance != null)
                {
                    ShopManager.instance.AddMoney(amountToAdd);
                    Destroy(gameObject);
                }
            }
        }
    }
}
