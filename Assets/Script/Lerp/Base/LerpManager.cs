using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;


/////////////////////////////////////////////////////////// 7/9
/// THE PLAN: 
/*
    1. Create SO lerp response handler or smth 
    2. Make a List of "enums" type that can be created dynamically ( aka just string that 
    act as pseudo enums) 
    3. Remember to choose the required data type that u want to use in the response handling 
    (Eg. if translate velocity need Vector3)
    4. Pass ts into lerp manager , get manager to extract all the data needed for this disaster 
    5. apply lerp at last 
*/

//the tech on how to use generics 101
public interface ILerpData
{
    //object stores the actual variable used 
    object value { get; }
    //type store the type so that it doesnt get lost in translation
    Type type { get; }
    void SetData(object newValue);
}

//then link the interface with the class 
[System.Serializable]
public class LerpData<T> : ILerpData
{

    T data;
    public object value => data;
    public Type type => typeof(T);

    public T GetTypedData()
    {
        return data;
    }

    public void SetData(object newValue)
    {
        if (newValue is T typedValue)
            data = typedValue;
    }
}


//makes it easier to sort through different types of lerp 
[System.Serializable]
public enum LERPTYPE
{
    TRANSLATE_VELOCITY,
    NUM_LERP
}


public class LerpManager : MonoBehaviour
{

    [SerializeField] private GameObject reference;
    [SerializeField] private List<LerpFunction> lerpList;
    private bool isActive;
    private bool areCoroutinesActived = false;

    private void Awake()
    {
        isActive = true;
        areCoroutinesActived = false;
        //disable all at first 
        foreach (LerpFunction lerp in lerpList)
        {
            lerp.isActive = false;
        }

    }


    //bad nested code but will work for now 
    void Update()
    {
        if (isActive)
        {
            //starting 
            if (!areCoroutinesActived)
            {
                foreach (LerpFunction lerp in lerpList)
                {
                    lerp.Init(reference);
                    StartCoroutine(lerp.ExcuteLerp(reference));
                }
                areCoroutinesActived = true; 
            }


            //do lerp values 
            foreach (LerpFunction lerp in lerpList)
            {
                LERPTYPE lerpType = lerp.lerpType;
                switch (lerpType)
                {
                    case LERPTYPE.TRANSLATE_VELOCITY:
                        //if (lerp.lerpData is LerpData<Vector3> vector3Lerp)
                        //    ResponseTranslateVelocity(vector3Lerp.GetTypedData());
                    break;
                }
            }
        }
        else
        {
            foreach (LerpFunction lerp in lerpList)
            {
                lerp.Reset();
            }
            StopAllCoroutines();
        }
    }



    //private void ResponseTranslateVelocity(Vector3 velocity) 
    //{
        
    //}

    private void OnDisable()
    {
        if (isActive)
        {
            isActive = false;
            foreach (LerpFunction lerp in lerpList)
            {
                lerp.Reset();
            }
            StopAllCoroutines();
        }
    }

    public void SetActiveStatus(bool condition)
    {
        isActive = condition;
    }
}
