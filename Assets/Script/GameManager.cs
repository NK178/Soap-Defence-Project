using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject mouseReference;
    [SerializeField] private ShopManager shopManagerReference;
    [SerializeField] private SpriteRenderer mouseImage;
    [SerializeField] private GridManager gridManagerReference;
    [SerializeField] private SpawnerManager spawnerManagerReference;

    private MousePositionReference mousePosReference;
    private OnMouseInteracts onMouseInteracts;
    private bool isGameRunning;
    private bool hasPlayerWon;
    private bool hasPlayerLost;

    //Debug 
    [SerializeField] private string DEBUGkeyName; 
    private InputAction DEBUGkey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mouseImage.enabled = false;
        isGameRunning = true;
        hasPlayerWon = false;
        hasPlayerLost = false;
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
    }

    private void OnDisable()
    {
        DEBUGkey.performed -= DEBUGHandleKey;

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

    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning)
            return; 


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
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else
        {
            mouseImage.enabled = false;
        }

        //Check for player win 
        if (spawnerManagerReference.areAllWavesSent)
        {
            //basically everything is gone
            if (spawnerManagerReference.AreAllSpawnsInactive() && spawnerManagerReference.AreAllSpawnersEmpty())
                TriggerPlayerWin();
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
