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

    public ShopItem Buy()
    {
        if (CheckIfCanBuy())
        {
           
            //technically not fully modular but it is what it 
            ShopItem myItem = (ShopItem)output.GetOutputObject();
            if (myItem != null)
                return myItem;
            else
                return null; 
        }
        else
        {
            return null; 
        }
    }


    public Sprite GetSprite()
    {
        return sprite; 
    }

}
