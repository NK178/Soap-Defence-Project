using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{

    [SerializeField] private MousePositionReference mousePosReference;
    [SerializeField] private SpriteRenderer mouseImage;
    //temp solution 
    [SerializeField] private Collider2D mouseCollider;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseImage.enabled = false;
        mouseCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldMousePos = mousePosReference.GetWorldMousePos();

        if (ShopManager.instance.isDragging)
        {
            ShopItem currentItem = ShopManager.instance.GetCurrentItem();
            SpriteRenderer sR = mouseImage.GetComponent<SpriteRenderer>();
            if (currentItem != null && sR != null)
            {
                sR.sprite = currentItem.GetSprite();
            }
            mouseImage.enabled = true;
            mouseCollider.enabled = true;
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else
        {
            mouseImage.enabled = false;
            mouseCollider.enabled = false;

        }
    }


    public void DebugCall()
    {
        Debug.Log("#### This is a Debug print ####");
    }
}
