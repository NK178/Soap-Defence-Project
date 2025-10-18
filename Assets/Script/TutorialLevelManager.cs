using NUnit.Framework;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UIColours{
    public Image imageUI;
    public Color defaultColour;
    public Color highlightedColour; 
}

public class TutorialLevelManager : LevelManager
{

    [SerializeField] private ShopItem soapKettleItem;
    [SerializeField] private ShopItem soapBarItem;
    [SerializeField] private UIColours bubbleAmountUI;
    [SerializeField] private UIColours soapKettleShopUI;

    private List<Entity> activeEntityReferenceList;

    private void Awake()
    {
        mouseImage.enabled = false;
        hasPlayerWon = false;
        hasPlayerLost = false;
        mousePosReference = mouseReference.GetComponent<MousePositionReference>();
        onMouseInteracts = mouseReference.GetComponent<OnMouseInteracts>();
        isGameRunning = false;
        activeEntityReferenceList = new List<Entity>();


        //load dialogue
        if (DialogueManager.instance != null)
            DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_0_INTRO);
        if (shopManagerReference != null)
        {
            shopManagerReference.AddItemIntoList(soapKettleItem);
            shopManagerReference.SetActiveStatus(false);
        }
    }

    private void Update()
    {
        HandleDialogue();

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


    private void HandleDialogue()
    {
        //dialogue sequencing 
        DIALOGUENAME currentDialogue = DialogueManager.instance.GetCurrentDialogue();
        switch (currentDialogue)
        {

            case DIALOGUENAME.TUTORIAL_0_INTRO:
                if (DialogueManager.instance.IsDialogueDone(DIALOGUENAME.TUTORIAL_0_INTRO))
                    DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_1_SHOP);
                break;
            case DIALOGUENAME.TUTORIAL_1_SHOP:

                // BAD METHOD BUT .................... hardcode for now lmao idk a good way to not hardcode it 
                if (DialogueManager.instance.GetDialogueQueueCount() == 1)
                    UIHighlightCurrency();
                else if (DialogueManager.instance.GetDialogueQueueCount() == 0)
                    UIHighlightShopItem();
                else if (DialogueManager.instance.IsDialogueDone(DIALOGUENAME.TUTORIAL_1_SHOP))
                {
                    DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_2_DRAG);
                    isGameRunning = true;
                    shopManagerReference.SetActiveStatus(true);
                }
                break;
            case DIALOGUENAME.TUTORIAL_2_DRAG:
                if (CheckIfEntityPresent(EntityNameList.ENTITYNAME.SOAPKETTLE))
                {
                    DialogueManager.instance.TriggerCurrentDialougeIfSpecial();
                    FindEntityFromActiveList(EntityNameList.ENTITYNAME.SOAPKETTLE).SetActiveStatus(false);
                    if (DialogueManager.instance.IsDialogueDone(DIALOGUENAME.TUTORIAL_2_DRAG))
                    {
                        DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_3_SOAPKETTLE);
                        FindEntityFromActiveList(EntityNameList.ENTITYNAME.SOAPKETTLE).SetActiveStatus(true);
                    }
                }
                break;
            case DIALOGUENAME.TUTORIAL_3_SOAPKETTLE:
                if (DialogueManager.instance.IsDialogueDone(DIALOGUENAME.TUTORIAL_3_SOAPKETTLE))
                    DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_4_SOAPKETTLE2);
                break;
            case DIALOGUENAME.TUTORIAL_4_SOAPKETTLE2:
                if (!CheckIfSoapBubblePresent())
                {
                    DialogueManager.instance.TriggerCurrentDialougeIfSpecial();
                    Entity entity = FindEntityFromActiveList(EntityNameList.ENTITYNAME.SOAPKETTLE);
                    if (entity != null)
                        entity.SetActiveStatus(false);
                    if (DialogueManager.instance.IsDialogueDone(DIALOGUENAME.TUTORIAL_4_SOAPKETTLE2))
                    {
                        DialogueManager.instance.StartDialogue(DIALOGUENAME.TUTORIAL_5_DRAGSOAPBAR);
                        shopManagerReference.AddItemIntoList(soapBarItem);
                    }
                }
                break;
            case DIALOGUENAME.TUTORIAL_5_DRAGSOAPBAR:
                if (CheckIfEntityPresent(EntityNameList.ENTITYNAME.SOAPBAR))
                {
                    DialogueManager.instance.TriggerCurrentDialougeIfSpecial();
                }
                break;
        }
    }

    private Entity FindEntityFromActiveList(EntityNameList.ENTITYNAME nameCheck)
    {
        foreach(Entity entity in activeEntityReferenceList)
        {
            if (entity.entityName == nameCheck)
            {
                return entity;
            }
        }

        return null; 
    }

    private bool CheckIfSoapBubblePresent()
    {
        CheckColliderDeleteSelf reference = FindFirstObjectByType<CheckColliderDeleteSelf>();
        if (reference != null)
        {
            if (reference.gameObject.name == "Bubble (Clone)")
                return true; 
        }
        return false;
    }

    private bool CheckIfEntityPresent(EntityNameList.ENTITYNAME referenceName)
    {
        if (gridManagerReference == null)
            return false;

        List<Entity> allEntities = new List<Entity>();
        gridManagerReference.GetListOfTypeInGrid(ref allEntities);
        foreach (Entity entity in allEntities)
        {
            if (entity.entityName == referenceName) {
                activeEntityReferenceList.Add(entity);
                return true;
            }
        }
        return false;
    }

    private void UIHighlightCurrency() 
    {
        bubbleAmountUI.imageUI.color = bubbleAmountUI.highlightedColour;
    }

    private void UIHighlightShopItem()
    {
        bubbleAmountUI.imageUI.color = bubbleAmountUI.defaultColour;
        soapKettleShopUI.imageUI.color = soapKettleShopUI.highlightedColour;

    }

    override public void StartGame()
    {
        isGameRunning = true;
        spawnerManagerReference.SetActiveStatus(true);
    }


}
