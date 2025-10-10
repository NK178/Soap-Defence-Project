using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject mouseReference;
    [SerializeField] private ShopManager shopManagerReference;
    [SerializeField] private SpriteRenderer mouseImage;
    [SerializeField] private GridManager gridManagerReference;
    [SerializeField] private SpawnerManager spawnerManagerReference;
    [SerializeField] private WrenchTool wrenchToolReference; 

    private MousePositionReference mousePosReference;
    private OnMouseInteracts onMouseInteracts;
    private bool isGameRunning;
    private bool hasPlayerWon;
    private bool hasPlayerLost;

    //Debug 
    [SerializeField] private string DEBUGkeyName;
    [SerializeField] private string wrenchKeyName;
    private InputAction DEBUGkey;
    private InputAction wrenchKey;
    private bool updateMouseSprite; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mouseImage.enabled = false;
        isGameRunning = true;
        hasPlayerWon = false;
        hasPlayerLost = false;
        updateMouseSprite = true;
        mousePosReference = mouseReference.GetComponent<MousePositionReference>();
        onMouseInteracts = mouseReference.GetComponent<OnMouseInteracts>();


    }

    private void OnEnable()
    {
        DEBUGkey = InputSystem.actions.FindAction(DEBUGkeyName);
        if (DEBUGkey != null)
        {
            DEBUGkey.started += DEBUGHandleKey;
            DEBUGkey.Enable();
        }
        wrenchKey = InputSystem.actions.FindAction(wrenchKeyName);
        if (wrenchKey != null)
        {
            wrenchKey.started += ToggleWrench;
            wrenchKey.Enable();
        }
    }

    private void OnDisable()
    {
        DEBUGkey.started -= DEBUGHandleKey;
        wrenchKey.started -= ToggleWrench;
    }

    // just used for debugging 
    private void DEBUGHandleKey(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            List<Entity> activeList = new List<Entity>();
            spawnerManagerReference.GetActiveSpawnsList(out activeList);
            foreach (Entity entity in activeList)
            {
                entity.SetEntityAliveStatus(false);
            }
        }   
    }

    private void ToggleWrench(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            wrenchToolReference.ToggleWrench();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning)
            return; 


        Vector3 worldMousePos = mousePosReference.GetWorldMousePos();


        if (ShopManager.instance.isDragging)
        {
            HandleMouseUIByShop();
            mouseImage.enabled = true;
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else if (wrenchToolReference.GetActiveStatus() && wrenchToolReference.IsObjectPickedUp())
        {
            HandleMouseUIByWrench();
            updateMouseSprite = false;
            mouseImage.enabled = true;
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else
        {
            mouseImage.enabled = false;
            updateMouseSprite = true;
        }

        //Check for player win 
        if (spawnerManagerReference.areAllWavesSent)
        {
            //basically everything is gone
            if (spawnerManagerReference.AreAllSpawnsInactive() && spawnerManagerReference.AreAllSpawnersEmpty())
                TriggerPlayerWin();
        }
    }

    private void HandleMouseUIByShop()
    {
        ShopItem currentItem = ShopManager.instance.GetCurrentItem();
        SpriteRenderer sR = mouseImage.GetComponent<SpriteRenderer>();
        if (currentItem != null && sR != null)
        {
            sR.sprite = currentItem.GetSprite();
            mouseImage.gameObject.transform.localScale = onMouseInteracts.defaultSpriteScale;
        }
    }

    private void HandleMouseUIByWrench()
    {
        Entity reference = wrenchToolReference.GetEntityReference();
        SpriteRenderer sR = mouseImage.GetComponent<SpriteRenderer>();
        float spriteScale = reference.transform.localScale.x; 
        if (reference != null)
        {
            sR.sprite = reference.GetEntitySprite();
            sR.color = reference.gameObject.GetComponentInChildren<SpriteRenderer>().color; //not needed I think
            sR.gameObject.transform.localScale = new Vector3(1/spriteScale, 1/spriteScale, 1);
        }
    }

    private void DeactiveDefences()
    {
        List<Entity> listEntityToDeactive = new List<Entity>();
        gridManagerReference.GetListOfTypeInGrid<Entity>(ref listEntityToDeactive);
        if (listEntityToDeactive.Count > 0)
        {
            foreach (Entity entity in listEntityToDeactive)
            {
                entity.SetEntityAliveStatus(false);
            }
        }
    }

    public void TriggerPlayerWin()
    {
        if (!isGameRunning)
            return;
        hasPlayerWon = true;
        hasPlayerLost = false;
        DeactiveDefences();
        Debug.Log("WIN");
        isGameRunning = false;
    }

    public void TriggerPlayerLost()
    {
        if (!isGameRunning)
            return;
        hasPlayerWon = false;
        hasPlayerLost = true;
        onMouseInteracts.SetActiveStatus(false);
        shopManagerReference.SetActiveStatus(false);
        DeactiveDefences();
        Debug.Log("LOSE");
        isGameRunning = false;
    }

    public void DebugCall()
    {
        Debug.Log("#### This is a Debug print ####");
    }
}
