using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;


// for programmer to choose 
public enum STATSTYPE
{ 
    HEALTH,
    DAMAGE, 
    NUM_STATS
}

public enum ENTITYTYPE
{
    TANK,
    SUPPORT,
    ATTACK,
    D_SOAP, //defences 
    E_GREASE, //enemies
    NUM_TYPE
}

public class Entity : MonoBehaviour
{
    [SerializeField] private CheckColliderByTag tagCollider;
    [SerializeField] private List<EntityStats> statsList;
    [SerializeField] private List<EntityFunctions> functionsList;
    [SerializeField] private List<EntityType> typeList;

    //to be used in calculation like health and whatever 
    private float[] currentStatsList; 


    private void Awake()
    {
        //run all the coroutines 
        for (int iter = 0; iter < functionsList.Count; iter++)
        {
            //to change to entity reference instead
            StartCoroutine(functionsList[iter].ExcuteCoroutine(this.gameObject));
        }

        currentStatsList = new float[statsList.Count];  
        for (int iter = 0; iter < statsList.Count; iter++)
        {
            currentStatsList[iter] = statsList[iter].GetValue();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //look for the values in the active float list not the stats template list 
    public float GetCurrentStatValue(STATSTYPE statType)
    {
        int index = 0;
        for (int iter = 0; iter < statsList.Count; iter++)
        {
            if (statsList[iter].GetStatType() == statType)
            {
                index = iter; 
                break;
            }
        }
        return currentStatsList[index];
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

    //public EntityType GetEntityTypeObject(ENTITYTYPE entityType)
    //{
    //    for (int iter = 0; iter < typeList.Count; iter++)
    //    {
    //        if (typeList[iter].GetEntityType() == entityType)
    //            return typeList[iter];
    //    }
    //    return null;
    //}

    //to find the enum and use in the type damage calculations 
    public EntityType GetMaterialType()
    {
        for (int iter = 0; iter < typeList.Count; iter++)
        {
            string enumString = typeList[iter].GetEntityType().ToString();
            if (enumString.StartsWith("D_") || enumString.StartsWith("E_"))
                return typeList[iter];
        }
        return null;
    }


    public void HandleDamage()
    {
        TypeInteractions typeInteraction = null;
        Entity refEntity = null;
        //check if it is a projectile with the type 
        if (tagCollider.currentColliding != null)
        {
            TypeInteractionData typeData = tagCollider.currentColliding.GetComponent<TypeInteractionData>();
            if (typeData != null)
                refEntity = typeData.GetReferenceEntity();

            if (refEntity != null)
                typeInteraction = TypeInteractionMap.instance.GetTypeInteraction(refEntity.GetMaterialType());
        }

        //compare attacker type to the defender type 
        if (typeInteraction != null)
        {
            float damageMultipler = 1f;
            EntityType selfMaterialType = GetMaterialType();
            if (selfMaterialType.GetEntityType() == typeInteraction.strongAgainst.GetEntityType())
                damageMultipler = 2f;
            else if (selfMaterialType.GetEntityType() == typeInteraction.weakAgainst.GetEntityType())
                damageMultipler = 0.5f;

            // apply damage 
            float baseDamage = refEntity.GetCurrentStatValue(STATSTYPE.DAMAGE);
            float totalDamage = baseDamage * damageMultipler;
            DealDamage(totalDamage);
        }
    }


    private void DealDamage(float damage)
    {
        float currentHealth = GetCurrentStatValue(STATSTYPE.HEALTH);
        currentHealth -= damage;
        Debug.Log("DAMAGE " + damage);
        Debug.Log("HEALTH " + currentHealth);
    }
}
