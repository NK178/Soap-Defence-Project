using Unity.VisualScripting;
using UnityEngine;

public class EnemyLayerHandler : MonoBehaviour
{
    
    [SerializeField] private int enemyLayer;
    [SerializeField] private CheckColliderByTag tagCollider; 

    public void RemoveLayer()
    {
        foreach (GameObject gameObject in tagCollider.allColliding)
        {
            if (gameObject.layer == enemyLayer)
            {
                gameObject.layer = 0;
            }
        }
    }

    public void AddPreviousLayer()
    {
        foreach (GameObject gameObject in tagCollider.allColliding)
        {
            if (gameObject.GetComponent<Entity>())
            {
                gameObject.layer = enemyLayer;
            }
        }
    }
    
}
