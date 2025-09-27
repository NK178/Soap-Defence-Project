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
        public EntityState state; 
    }
    [SerializeField] private List<TransitionState> transitionList; 

    public void UpdateState(Entity entity)
    {
        //ExcuteCoroutine(Entity) 
        
    }

    public void CheckTransition()
    {
        for (int i = 0; i < transitionList.Count; i++)
        {
            if (transitionList[i].decision.DecisionCheck())
            {
                return transitionList[i].state; 
            }
        }
        return null;
    }

    //need to have run coroutine funciton bruh 


}
