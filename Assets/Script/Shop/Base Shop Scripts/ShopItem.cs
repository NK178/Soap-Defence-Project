using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/ShopItem")]
public class ShopItem : ScriptableObject
{


    [SerializeField] private List<ShopItemCondition> conditions;
    [SerializeField] private IShopItemOutput output;

    [HideInInspector] public bool isBought;


    void Awake()
    {
        isBought = false;
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

    public ShopItem Buy()
    {
        if (CheckIfCanBuy())
        {
            isBought = true;
            ShopItem temp = null;
            output.GetOutput();
        }
        else
        {
            return null; 
        }
    }



    }
