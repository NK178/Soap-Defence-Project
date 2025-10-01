using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[CreateAssetMenu(fileName = "EntityState", menuName = "Scriptable Objects/EntityState")]
public class EntityState : ScriptableObject
{

    [System.Serializable]
    public class TransitionState {
        public EntityStateDecision decision;
        public EntityState trueState; 
        public EntityState falseState; 
    }

    [SerializeField] private List<EntityFunctions> functionsList; 
    [SerializeField] private List<TransitionState> transitionsList;


    public void Init(Entity entity)
    {
        ExcuteFunctions(entity);
        ExcuteDecisionCoroutine(entity);
    }

    //i might do coroutine to this via the entity
    public void UpdateState(Entity entity)
    {
        CheckTransition(entity);       
    }

    private void ExcuteDecisionCoroutine(Entity entity)
    {
        for (int i = 0; i < transitionsList.Count; i++)
        {
            entity.StartCoroutine(transitionsList[i].decision.ExcuteCoroutine(entity));
        }
    }

    private void ExcuteFunctions(Entity entity)
    {
        for (int i = 0; i < functionsList.Count; i++)
        {
            entity.StartCoroutine(functionsList[i].ExcuteCoroutine(entity));
        }
    }

    private void CheckTransition(Entity entity)
    {
        for (int i = 0; i < transitionsList.Count; i++)
        {
            //stop all coroutines before changing to a new state(unless remain state) 
            bool decisionSucceed = transitionsList[i].decision.DecisionCheck(entity);
            EntityState newState = new EntityState();
            if (decisionSucceed)
                newState = transitionsList[i].trueState;
            else 
                newState = transitionsList[i].falseState;

            if (newState.name != "RemainState")
                StopAllFunctions(entity);
            entity.TransitionState(newState);
        }
    }
    
    //this will stop main coroutine so its kinda bad 
    private void StopAllFunctions(Entity entity)
    {
        entity.StopAllCoroutines();
    }
}
