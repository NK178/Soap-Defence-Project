using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class LevelManager : MonoBehaviour
{

    [SerializeField] private GameObject mouseReference;
    [SerializeField] private ShopManager shopManagerReference;
    [SerializeField] private SpriteRenderer mouseImage;
    [SerializeField] private GridManager gridManagerReference;
    [SerializeField] private SpawnerManager spawnerManagerReference;
    [SerializeField] private WrenchTool wrenchToolReference; 
    [SerializeField] private SawTool sawToolReference; 

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mouseImage.enabled = false;
        isGameRunning = false;
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
            wrenchToolReference.SetWrenchActive(false);
            HandleMouseUIByShop();
            mouseImage.enabled = true;
            mouseImage.gameObject.transform.position = worldMousePos;
        }
        else if (wrenchToolReference.GetWrenchActiveStatus())
        {
            HandleMouseUIByWrench();
            mouseImage.enabled = true;
            mouseImage.gameObject.transform.position = worldMousePos;
            shopManagerReference.SetBubbleCollectionStatus(false);
            //sawToolReference.SetSawActive(false);
        }
        else if (sawToolReference.GetSawActiveStatus())
        {
            shopManagerReference.SetBubbleCollectionStatus(false);
            //wrenchToolReference.SetWrenchActive(false);
        }
        else
        {
            mouseImage.enabled = false;
            shopManagerReference.SetBubbleCollectionStatus(true);

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
        SpriteRenderer sR = mouseImage.GetComponent<SpriteRenderer>();
        //if object is not picked up do glove sprite 
        if (!wrenchToolReference.IsObjectPickedUp())
        {
            sR.sprite = onMouseInteracts.defaultSprite;
            onMouseInteracts.StopAnimatorAndClear();
            mouseImage.gameObject.transform.localScale = onMouseInteracts.defaultSpriteScale;
        }
        else
        {
            Entity reference = wrenchToolReference.GetEntityReference();
            Animator entityAnimator = null;
            if (reference != null)
                entityAnimator = reference.GetAnimator();

            float spriteScale = reference.transform.localScale.x;
            if (entityAnimator != null)
            {
                onMouseInteracts.SetAndPlayDefaultAnim(entityAnimator);
                sR.gameObject.transform.localScale = new Vector3(1 / spriteScale, 1 / spriteScale, 1);
            }
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
                entity.SetActiveStatus(false);
                entity.StopAllCoroutines();
            }
        }
    }

    private void DeactiveEnemies()
    {
        List<Entity> activeList = new List<Entity>();
        spawnerManagerReference.GetActiveSpawnsList(out activeList);
        foreach (Entity entity in activeList)
        {
            entity.SetActiveStatus(false);
            entity.StopAllCoroutines();
        }
    }

    public void TriggerPlayerWin()
    {
        if (!isGameRunning)
            return;
        hasPlayerWon = true;
        hasPlayerLost = false;
        wrenchToolReference.SetActiveStatus(false);
        spawnerManagerReference.SetActiveStatus(false);
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
        wrenchToolReference.SetActiveStatus(false);
        spawnerManagerReference.SetActiveStatus(false); 
        DeactiveDefences();
        DeactiveEnemies();
        Debug.Log("LOSE");
        isGameRunning = false;
    }

    public bool GetUtilitiesActiveStatus()
    {
        if (wrenchToolReference.GetWrenchActiveStatus() || sawToolReference.GetSawActiveStatus())
            return true;
        else
            return false;
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public bool DidPlayerWin()
    {
        return hasPlayerWon;
    }

    public bool DidPlayerLose()
    {
        return hasPlayerLost;
    }

    public void StartGame()
    {
        //check if the player has brought enough defences 
        if (CanStartGame())
        {
            isGameRunning = true;
            spawnerManagerReference.SetActiveStatus(true);
        }
        else
        {
            Debug.Log("NOT ENOUGH DEFENCES");
        }
    }

    public bool CanStartGame()
    {
        return shopManagerReference.areDefencesEnough;
    }

    public void DebugCall()
    {
        Debug.Log("#### This is a Debug print ####");
    }
}
