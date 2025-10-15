using UnityEngine;

public class InventoryUIHandler : MonoBehaviour
{

    [SerializeField] private GameObject inventoryUI; 

    public void TriggerStartLevel()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.CanStartLevel())
            {
                GameManager.instance.StartLevel();
                inventoryUI.SetActive(false);
                gameObject.SetActive(false);
                Debug.Log("STARTING LEVEL");
            }
            else
                Debug.Log("CANT START LEVEL");
        }
    }
}
