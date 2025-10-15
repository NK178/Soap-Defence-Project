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
    }

    // Update is called once per frame
    void Update()
    {
        
       switch (currentState)
       {
            case GAMESTATES.PLAY:
                HandlePlayState();
                break;
       }
        
    }

    private void HandlePlayState()
    {
        //Play button willl trigger level to start 
        if (currentLevelManager == null)
            currentLevelManager = FindAnyObjectByType<LevelManager>();
        else
        {
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
            currentLevelManager.StartGame();
    }


    //public bool HandleStartLevel()
    //{
    //    if (currentLevelManager != null)
    //    {
    //        if (currentLevelManager.CanStartGame())
    //        {
    //            currentLevelManager.StartGame();
    //            return true; 
    //        }
    //    }
    //    return false; 
    //}

    public void StartPlay()
    {
        //set up scene manager here 
        //for now temp is go to sampleScene
        SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene("InventoryScene",LoadSceneMode.Additive);

        currentState = GAMESTATES.PLAY;
    }

    public void ChangeSceneTest(string name)
    {
        SceneManager.LoadScene(name);
    }
}
