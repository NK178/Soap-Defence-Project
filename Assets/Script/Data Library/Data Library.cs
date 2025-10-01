using System.Collections.Generic;
using UnityEngine;


//THIS IS NOT A SINGLETON DESIGN, not a univeral data holder, can be used in many other places for different purposes
public class DataLibrary
{
    

    // 1/10 I could make this modular and verstile but for now I wont do any premature optimisation 


    private Dictionary<int, bool> BoolValuePairs = new Dictionary<int, bool>(); 


    public void AddBool(int key, bool value)
    {
        bool result = CheckIfKeyExists(key);
        if (result)
            BoolValuePairs[key] = value;
        else
            BoolValuePairs.Add(key, value);
    }


    public void SetBool(int key, bool value)
    {
        BoolValuePairs[key] = value;        
    }

    public bool GetBool(int key)
    {
        BoolValuePairs.TryGetValue(key, out bool result);
        return result; 
    }

    public bool CheckIfKeyExists(int key)
    {
        return BoolValuePairs.ContainsKey(key);
    }


}
