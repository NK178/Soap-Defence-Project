using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


// for programmer to choose 
public enum STATSTYPE
{ 
    HEALTH,
    DAMAGE, 
    NUM_STATS
}

public enum ENTITYTYPE
{
    DEFENCE,
    SUPPORT,
    ATTACK,
    NUM_TYPE
}

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
            StartCoroutine(functionsList[iter].ExcuteCoroutine(this.gameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float GetStatValue(STATSTYPE statType)
    {
        float returnValue = -1f;
        for (int iter = 0; iter < statsList.Count; iter++)
        {
            if (statsList[iter].GetStatType() == statType)
            {
                returnValue = statsList[iter].GetValue();
                break; 
            }
        }

        return returnValue; 
    }


    public bool CheckIfThisEntityType(ENTITYTYPE entityType)
    {
        bool isTypeCorrect = false;
        for (int iter = 0; iter < typeList.Count; iter++)
        {
            if (typeList[iter].GetEntityType() == entityType)
            {
                isTypeCorrect = true;
                break;
            }
        }
        return isTypeCorrect;
    }
}
