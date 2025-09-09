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

public class LerpManager : MonoBehaviour
{

    //dont use field reference cos uhh it will reference all prefabs
    private GameObject reference;
    [SerializeField] private List<LerpFunction> lerpFunctionTemplates;
    [SerializeField] private List<LerpResponse> lerpResponseTemplates;
    private bool isActive;
    private bool areCoroutinesActived = false;

    //The actual list used in the lerp 
    private List<LerpFunction> lerpFunctionList;
    private List<LerpResponse> lerpResponseList;

    private void Awake()
    {
        isActive = true;
        areCoroutinesActived = false;
        reference = gameObject;

        //creating instances of the scriptable object so that they are able to run seperately and not cause bugs 
        lerpFunctionList = new List<LerpFunction>();
        foreach (LerpFunction template in lerpFunctionTemplates)
        {
            ////need to init first 
            //template.Init(reference);
            //need to get the concrete type as u cant create instance of abstract class 
            Type concreteType = template.GetType();
            LerpFunction copy = ScriptableObject.CreateInstance(concreteType) as LerpFunction;
            copy.CopyClassData(template);
            copy.isActive = false;
            lerpFunctionList.Add(copy);
        }
        lerpResponseList = new List<LerpResponse>();
        foreach (LerpResponse template in lerpResponseTemplates)
        {
            Type concreteType = template.GetType();
            LerpResponse copy = ScriptableObject.CreateInstance(concreteType) as LerpResponse;
            copy.CopyClassData(template);
            lerpResponseList.Add(copy);
        }

    }


    //bad nested code but will work for now 
    void Update()
    {
        if (isActive)
        {
            ActiveCoroutines();
            AddResponseData();
            ResolveResponseData();
        }
        else
        {
            foreach (LerpFunction lerp in lerpFunctionList)
            {
                lerp.Reset();
            }
            StopAllCoroutines();
        }
    }

    private void ActiveCoroutines()
    {
        if (!areCoroutinesActived)
        {
            foreach (LerpFunction lerp in lerpFunctionList)
            {
                lerp.Init(reference);
                StartCoroutine(lerp.ExcuteLerp(reference));
            }
            areCoroutinesActived = true;
        }
    }

    private void AddResponseData()
    {
        foreach (LerpFunction lerpFunc in lerpFunctionList)
        {
            LerpResponse lerpResponse = null;
            foreach (LerpResponse lerpRes in lerpResponseList)
            {
                if (lerpRes.lerpType.stringName == lerpFunc.lerpType.stringName)
                {
                    Debug.Log("ARE U RUNNING");
                    lerpResponse = lerpRes;
                    break;
                }
            }

            if (lerpResponse != null)
            {
                lerpResponse.AddValueToCalculation(lerpFunc.lerpData);
            }
        }
    }

    private void ResolveResponseData()
    {
        foreach (LerpResponse lerpRes in lerpResponseList)
        {
            lerpRes.HandleResponse(reference);
        }
    }

    private void OnDisable()
    {
        if (isActive)
        {
            isActive = false;
            foreach (LerpFunction lerp in lerpFunctionList)
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
