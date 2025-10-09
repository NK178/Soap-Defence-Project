using System;
using System.Collections.Generic;


// 7/10 I should see if I can make this method better but for now it should work 
//because I think its possible to do this without the interfaces maybe 

//base non generic, to use with base wrapper class so that can accept data
abstract public class FusionDataBase {
    abstract public Type GetFusionType();
}

//the actual thing 
public class FusionData<T> : FusionDataBase
{
    public List<T> inputList;
    public List<T> outputList;
    public FusionDecision<T> fusionDecision;
    public FusionResponse<T> fusionResponse;

    public bool IsFusionValid(List<T> actualInputList)
    {
        bool result = fusionDecision.IsFusionValid(inputList, actualInputList);
        return result; 
    }

    public void ResolveFusion(List<T> actualInputList)
    {
        fusionResponse.HandleFusionResponse(outputList, actualInputList);
    }

    public override Type GetFusionType()
    {
        return typeof(T);
    }
}


public interface IFusionDataWrapper
{
    FusionDataBase GetFusionData(); 
}

public interface IFusionDataWrapper<T> : IFusionDataWrapper
{
    FusionData<T> fusionData { get; set; }
}

////interface, used to input the data type into fusion data (in the wrapper class)
//interface IFusionDataWrapper<T> : IFusionDataWrapper
//{
//    //this bracket thingy means the class implementing this must have a method to set and get it, 
//    // also must do it this way because this variable is a property not a value in this interface as 
//    //interfaces should not store data 
//    FusionData<T> fusionData { get; set; }
//}

////Non generic interface so that I can access the stuff 
//interface IFusionDataWrapper {
//    bool IsTypeSame(Type type);
//}

