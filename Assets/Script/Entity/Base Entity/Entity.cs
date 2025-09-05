using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private List<EntityStats> statsList;
    [SerializeField] private List<EntityFunctions> functionsList;
    [SerializeField] private List<EntityType> typeList;


    private void Awake()
    {
        //run all the coroutines 
        for (int iter = 0; iter < functionsList.Count; iter++)
        {
            StartCoroutine(functionsList[iter].coroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
