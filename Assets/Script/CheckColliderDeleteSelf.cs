using System.Collections.Generic;
using UnityEngine;

public class CheckColliderDeleteSelf : MonoBehaviour
{
    [SerializeField] private List<string> tagNameList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tagNameList.Count; i++)
        {
            if (collision.gameObject.CompareTag(tagNameList[i]))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
