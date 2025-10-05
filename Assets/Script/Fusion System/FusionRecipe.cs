using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionRecipe", menuName = "Scriptable Objects/FusionRecipe")]
public class FusionRecipe<T> : IFusionData
{

    [SerializeField] private List<T> inputList;
    [SerializeField] private List<T> outputList;
    private List<T> currentInputlist;

    public Type type => typeof(T);


    //5/10 Hmmmmm how to compare between templates 
    //public bool IsFusionValid()
    //{
    //    if (currentInputlist.Count != inputList.Count)
    //        return false;

    //    for (int iter = 0; iter < currentInputlist.Count; iter++)
    //    {
    //        if (currentInputlist[iter] == inputList[iter])
    //    }
    //}
    
}
