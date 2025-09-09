using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


//basically a global class to get mouse position from 
// u need this or u die
// use inspector reference is better I think
public class MousePositionReference : MonoBehaviour
{


    [SerializeField] private Camera cam;
    Mouse mouse;


    private void Awake()
    {
        mouse = Mouse.current;
    }

    public Vector3 GetWorldMousePos()
    {
        Vector2 mousePos = mouse.position.ReadValue();
        Vector3 worldMousePos = new Vector3(cam.ScreenToWorldPoint(mousePos).x, cam.ScreenToWorldPoint(mousePos).y, 0);
        return worldMousePos;
    }
}
