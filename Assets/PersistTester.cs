using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistTester : MonoBehaviour
{


    public static PersistTester instance; 


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
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void ChangeSceneTest(string name)
    {
        SceneManager.LoadScene(name);
    }
}
