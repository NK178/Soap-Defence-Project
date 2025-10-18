using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;




public enum DIALOGUENAME {
    TUTORIAL_0_INTRO,
    TUTORIAL_1_SHOP,
    TUTORIAL_2_DRAG,
    TUTORIAL_3_SOAPKETTLE,
    TUTORIAL_4_SOAPKETTLE2,
    TUTORIAL_5_DRAGSOAPBAR,
    NUM_DIALOGUE
}

public enum DIALOGUERESOLVE
{
    TIME,
    BUTTON,
    SPECIAL, //aka handle elsewhere
    NUM_RESPONSES
}

[System.Serializable]
public class  DialogueKey 
{
    public DialogueContent content;
    public DIALOGUENAME key; 
}


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] private List<DialogueKey> dialogueList;
    private Queue<string> dialogueQueue;
    private DialogueKey currentDialogueKey; 
    private string currentActiveText;

    private bool isActive;
    private bool isDialogueRunning;
    private bool shouldChangeSentence;
    public static DialogueManager instance { get; private set; }


    void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        isActive = true;
        isDialogueRunning = shouldChangeSentence = false;
        currentActiveText = dialogue.text = string.Empty;
        dialogueQueue = new Queue<string>();
        dialogueButton.SetActive(false);

        //Debug
        //StartDialogue(DIALOGUENAME.TUTORIAL_INTRO);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if (isDialogueRunning) 
        {
            dialogue.text = currentActiveText;
        }

        //Debug.Log(currentDialogueKey.key.ToString());
        Debug.Log("DIALOGUE: " + isDialogueRunning);
    }

    private IEnumerator RunSentence(string sentence)
    {
        while (true)
        {
            for (int i = 0; i < sentence.Length; i++)
            {
                currentActiveText += sentence[i];
                yield return new WaitForSeconds(currentDialogueKey.content.textSpeed);
            }
            break;
        }
    }

    //17/10 this is ugly , I can prob make run dialogue a normal function, then a ienumerator to handle the dialogue resolve types 
    private IEnumerator RunDialogue()
    {
        if (dialogueQueue.Count == 0)
            yield break;

        bool isDialogueDone = false;
        while (!isDialogueDone)
        {
            dialogueBox.SetActive(true);
            isDialogueRunning = true;
            //need to start pause only after the text is fully out 
            string sentence = dialogueQueue.Dequeue();
            shouldChangeSentence = false;
            yield return StartCoroutine(RunSentence(sentence)); //apparently this waits for the coroutine to finish

            if (currentDialogueKey.content.resolve == DIALOGUERESOLVE.TIME)
            {
                yield return new WaitForSeconds(currentDialogueKey.content.pauseTime);
                shouldChangeSentence = true;
            }
            else if (currentDialogueKey.content.resolve == DIALOGUERESOLVE.BUTTON)
            {
                dialogueButton.SetActive(true);
                while (!shouldChangeSentence)
                {
                    yield return null;
                }
                dialogueButton.SetActive(false); 
            }
            else if (currentDialogueKey.content.resolve == DIALOGUERESOLVE.SPECIAL)
            {
                //handle elsewhere
                while (!shouldChangeSentence)
                {
                    yield return null;
                }
            }


            if (shouldChangeSentence)
            {
                currentActiveText = string.Empty;
                if (dialogueQueue.Count == 0)
                {
                    isDialogueDone = true;
                    isDialogueRunning = false;
                    dialogueBox.SetActive(false);
                }
            }
        }
    }

    private void LoadDialogue(string content)
    {
        string sentence = string.Empty;

        //split up the sentences into the queue 
        for(int i = 0; i < content.Length; i++)
        {
            sentence += content[i];
            if (content[i] == '.' || content[i] == '!')
            {
                dialogueQueue.Enqueue(sentence);
                sentence = string.Empty;
            }
        }
    }

    public void StartDialogue(DIALOGUENAME dialogueName)
    {
        foreach(DialogueKey keyValue in dialogueList)
        {
            if (keyValue.key == dialogueName)
            {
                currentDialogueKey = keyValue; 
                LoadDialogue(keyValue.content.text);
                break;
            }
        }
        StartCoroutine(RunDialogue());
    }

    //CHECK THIS 17/10
    public bool IsDialogueDone(DIALOGUENAME dialogueName)
    {
        if (currentDialogueKey != null && !isDialogueRunning)
        {
            if (currentDialogueKey.key == dialogueName && dialogueQueue.Count == 0)
                return true; 
        }
        return false;
    }

    public DIALOGUENAME GetCurrentDialogue()
    {
        return currentDialogueKey.key; 
    }

    public int GetDialogueQueueCount()
    {
        if (isDialogueRunning)
            return dialogueQueue.Count;
        else
            return -1;
    }

    public void TriggerCurrentDialougeIfSpecial()
    {
        if (currentDialogueKey != null)
        {
            if (currentDialogueKey.content.resolve == DIALOGUERESOLVE.SPECIAL)
                shouldChangeSentence = true;
            else
                Debug.Log("FAILED TO CHANGE DIALOGUE");
        }
    }

    public void ManualDialogueTrigger()
    {
        shouldChangeSentence = true;
    }
}
