using Microsoft.Win32.SafeHandles;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/ShopItem")]
public class ShopItem : ScriptableObject
{

    [SerializeField] private Sprite sprite;
    [SerializeField] private List<ShopItemCondition> conditions;
    [SerializeField] private ShopItemOutput output;



    void Awake()
    {

    }

    public bool CheckIfCanBuy()
    {

        bool canBuy = true;
        for (int iter = 0; iter < conditions.Count; iter++)
        {
            if (!conditions[iter].IsValid())
            {
                canBuy = false;
                break;
            }
        }

        return canBuy;
    }

    //let whoever is handling the buy do the casting 
    public object Buy()
    {
        object myOutput = null;
        if (CheckIfCanBuy())
            myOutput = output.GetOutputObject();
       
        if (myOutput != null)
            return myOutput;
        else
            return null;
    }

    public void HandleConditionResponse()
    {
        for (int iter = 0; iter < conditions.Count; iter++)
        {
            conditions[iter].ConditionResolve();
        }
    }

    public Sprite GetSprite()
    {
        return sprite; 
    }




}
