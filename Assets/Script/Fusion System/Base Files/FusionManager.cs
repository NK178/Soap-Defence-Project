using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class FusionDataWrapper : ScriptableObject, IFusionDataWrapper
{
    public virtual FusionDataBase GetFusionData() => null;
}


public class FusionManager : MonoBehaviour
{

    [SerializeField] private List<FusionDataWrapper> fusionRecipeList;
    static public FusionManager instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    public void TriggerFusionIfValid<T>(List<T> refList)
    {
        //first find compatible data type with the list,if find cast the base data into the specific ones 
        List<FusionData<T>> validTypeDataList = new List<FusionData<T>>(); 
        for (int iter = 0; iter < fusionRecipeList.Count; iter++)
        {
            FusionDataBase baseData = fusionRecipeList[iter].GetFusionData();
            if (baseData != null)
            {
                if (baseData.GetFusionType() == typeof(T))
                    validTypeDataList.Add((FusionData<T>)baseData);
            }
        }

        bool result = false;
        FusionData<T> typeFusionData = null;
        //now with the type specific data, go check if fusion valid
        for (int iter = 0; iter < validTypeDataList.Count; iter++)
        {
            if (validTypeDataList[iter].IsFusionValid(refList))
            {
                result = true;
                typeFusionData = validTypeDataList[iter];
                break;
            }
        }

        if (result)
        {
            typeFusionData.ResolveFusion(refList);
            Debug.Log("FUSION VALID");
        }
        else
            Debug.Log("FUSION INVALID U DUM");
    }

}
