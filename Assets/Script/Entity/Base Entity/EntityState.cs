using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "EntityState", menuName = "Scriptable Objects/EntityState")]
public class EntityState : ScriptableObject
{

    [System.Serializable]
    public class TransitionState {
        public EntityStateDecision decision;
        //public EntityState state; 
        public EntityState trueState; 
        public EntityState falseState; 
    }

    [SerializeField] private List<EntityFunctions> functionsList; 
    [SerializeField] private List<TransitionState> transitionsList;
    private bool isFunctionsActive = false; 


    public void Init()
    {
        isFunctionsActive = false;
    }

    //i might do coroutine to this via the entity
    public void UpdateState(Entity entity)
    {
        //only trigger once else bad things will happen 
        if (!isFunctionsActive)
        {
            Debug.Log("FUNCTION EXCUTING");
            ExcuteFunctions(entity);
            isFunctionsActive = true;   
        }
        CheckTransition(entity);
            
    }

    public void ExcuteFunctions(Entity entity)
    {
        for (int i = 0; i < functionsList.Count; i++)
        {
            entity.StartCoroutine(functionsList[i].ExcuteCoroutine(entity.gameObject));
        }
    }


    public void CheckTransition(Entity entity)
    {
        for (int i = 0; i < transitionsList.Count; i++)
        {
            //stop all coroutines before changing to a new state(unless remain state) 
            bool decisionSucceed = transitionsList[i].decision.DecisionCheck(entity);
            if (decisionSucceed)
            {
                if (transitionsList[i].trueState.name != "RemainState")
                    StopAllFunctions(entity);
                entity.TransitionState(transitionsList[i].trueState);
            }
            else
            {
                if (transitionsList[i].falseState.name != "RemainState")
                    StopAllFunctions(entity);
                entity.TransitionState(transitionsList[i].falseState);
            }


            //string thisSOName = AssetDatabase.GetAssetPath(this);
            /////////////// old way 
            //if (transitionsList[i].decision.DecisionCheck(entity))
            //{
            //    StopAllFunctions(entity);
            //    entity.TransitionState(transitionsList[i].state);
            //    break;
            //}
        }
    }
    
    //this will stop main coroutine so its kinda bad 
    private void StopAllFunctions(Entity entity)
    {
        entity.StopAllCoroutines();
        isFunctionsActive = false;
    }
}
