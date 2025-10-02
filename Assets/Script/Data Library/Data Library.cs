using System.Collections.Generic;
using UnityEngine;


//THIS IS NOT A SINGLETON DESIGN, not a univeral data holder, can be used in many other places for different purposes
public enum DATATYPE
{
    BOOLEAN, 
    FLOAT,
    VECTOR3,
    NUM_DATATYPES
}
public class DataLibrary
{
    

    // 1/10 I could make this modular and verstile but for now I wont do any premature optimisation 


    private Dictionary<int, bool> boolValuePairs = new Dictionary<int, bool>(); 
    private Dictionary<int, float> floatValuePairs = new Dictionary<int, float>();
    private Dictionary<int, Vector3> vector3ValuePairs = new Dictionary<int, Vector3>();

    public void AddVector3(int key, Vector3 value)
    {
        bool result = CheckIfKeyExists(key, DATATYPE.VECTOR3);
        if (result)
            vector3ValuePairs[key] = value;
        else
            vector3ValuePairs.Add(key, value);
    }

    public void SetVector3(int key, Vector3 value)
    {
        vector3ValuePairs[key] = value;
    }

    public Vector3 GetVector3(int key)
    {
        vector3ValuePairs.TryGetValue(key, out Vector3 result);
        return result;
    }

    public void AddFloat(int key, float value)
    {
        bool result = CheckIfKeyExists(key, DATATYPE.FLOAT);
        if (result)
            floatValuePairs[key] = value;
        else
            floatValuePairs.Add(key, value);
    }

    public void SetFloat(int key, float value)
    {
        floatValuePairs[key] = value;
    }

    public float GetFloat(int key)
    {
        floatValuePairs.TryGetValue(key, out float result);
        return result;
    }


    public void AddBool(int key, bool value)
    {
        bool result = CheckIfKeyExists(key,DATATYPE.BOOLEAN);
        if (result)
            boolValuePairs[key] = value;
        else
            boolValuePairs.Add(key, value);
    }

    public void SetBool(int key, bool value)
    {
        boolValuePairs[key] = value;        
    }

    public bool GetBool(int key)
    {
        boolValuePairs.TryGetValue(key, out bool result);
        return result; 
    }

    public bool CheckIfKeyExists(int key, DATATYPE dataType)
    {
        bool result = false; 
        switch (dataType)
        {
            case DATATYPE.BOOLEAN:
                result = boolValuePairs.ContainsKey(key);
                break; 
            case DATATYPE.FLOAT:
                result = floatValuePairs.ContainsKey(key);
                break;
            case DATATYPE.VECTOR3:
                result = vector3ValuePairs.ContainsKey(key);
                break;
        }
        return result; 
    }


}
