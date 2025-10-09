using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{


    private List<GameObject> gridList; 


    void Awake()
    {
        //if this fails its basically ggs
        gridList = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            gridList.Add(child);
        }         
    }

    public void GetListOfTypeInGrid<T>(ref List<T> refList)
    {
        foreach (GameObject child in gridList)
        {
            //check for child of type 
            foreach (Transform secondChild in child.transform)
            {
                T component = secondChild.GetComponent<T>();
                if (component != null)
                {
                    Debug.Log("FOUND DEFENCE: " + secondChild.name);
                    refList.Add(secondChild.GetComponent<T>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
