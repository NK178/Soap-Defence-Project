using System.Collections.Generic;


public class FusionData<T>
{
    public List<T> inputList;
    public List<T> outputList;
    public FusionDecision<T> fusionDecision;

    public bool IsFusionValid(List<T> actualInputList)
    {
        bool result = fusionDecision.IsFusionValid(inputList, actualInputList);
        return result; 
    }
}

//interface 
interface IFusionDataWrapper<T>
{
    //this bracket thingy means the class implementing this must have a method to set and get it, 
    // also must do it this way because this variable is a property not a value in this interface as 
    //interfaces should not store data 
    FusionData<T> fusionData { get; set; }
}