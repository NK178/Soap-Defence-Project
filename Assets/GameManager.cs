using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GAMESTATES {
    HOMESCREEN, 
    PLAY,
    MENU,
    WIKI,
    SETTINGS, 
    CREDITS,
    NUM_STATES
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private FloatSO isTutorialPlayedBool;
    public static GameManager instance;

    private LevelManager currentLevelManager;
    private GAMESTATES currentState; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        currentState = GAMESTATES.HOMESCREEN;
        currentLevelManager = null;

        //idk how to initalize the tutorial played boolean but I leave it like this for now 
        //isTutorialPlayedBool
    }

    // Update is called once per frame
    void Update()
    {
        
       switch (currentState)
       {
            case GAMESTATES.MENU:
                HandleMenuState();
                break;
            case GAMESTATES.PLAY:
                HandlePlayState();
                break;
       }
        
    }

    private void HandleMenuState()
    {
        //prepping to enter play mode 
        if (currentLevelManager == null)
            currentLevelManager = FindAnyObjectByType<LevelManager>();
    }

    private void HandlePlayState()
    {
        //effectivly this is the game loop 
        if (currentLevelManager != null) { 
            bool isGameRunning = currentLevelManager.IsGameRunning();

            //status can be false before/after game 
            if (!isGameRunning)
            {
                if (currentLevelManager.DidPlayerWin())
                {
                    //mark level for completion? 
                    Debug.Log("Player Win");
                }
                else if (currentLevelManager.DidPlayerLose())
                {
                    //allow player to restart level? 
                }
                //menu 
                //else
                //{
                //    //plant selector unless tutorial 
                //    //temp trigger 
                //    currentLevelManager.StartGame();
                //}
            }
        }
    }

    public bool CanStartLevel()
    {
        if (currentLevelManager != null)
            return currentLevelManager.CanStartGame();
        else
            return false; 
    }

    public void StartLevel()
    {
        if (currentLevelManager != null)
        {
            currentLevelManager.StartGame();
            currentState = GAMESTATES.PLAY; 
        }
    }


    public void StartMenu()
    {
        //set up scene manager here 

        //if tutorial has not been played
        if (isTutorialPlayedBool.value == 0f)
        {
            //load dialogue scene first incase of null issues 
            SceneManager.LoadScene("DialogueScene");
            SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);
        }
        else
        {
            //for now temp is go to sampleScene
            SceneManager.LoadScene("SampleScene");
            SceneManager.LoadScene("InventoryScene", LoadSceneMode.Additive);
        }


        //dont enter game mdoe first 
        currentState = GAMESTATES.MENU;
    }   

    public GAMESTATES GetCurrentGameState()
    {
        return currentState; 
    }

    public bool IsInTutorialMode()
    {
        //if have not played tutorial 
        if (isTutorialPlayedBool.value == 0f)
            return true;
        else
            return false;
    }
    public void ChangeSceneTest(string name)
    {
        SceneManager.LoadScene(name);
    }

    
}
