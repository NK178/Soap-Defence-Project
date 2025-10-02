using System.Collections.Generic;
using UnityEngine;


// for programmer to choose 
public enum STATSTYPE
{ 
    HEALTH,
    DAMAGE, 
    NUM_STATS
}

public enum ENTITYTYPE
{
    TANK,
    SUPPORT,
    ATTACK,
    D_SOAP, //defences 
    E_GREASE, //enemies
    E_DIRT,
    NUM_TYPE
}


public enum ENTITYANIMS { 
    DEFAULT,
    ATTACK, 
    SPECIAL,
    NUM_ANIMS
}

public class Entity : MonoBehaviour
{
    [SerializeField] private CheckColliderByTag tagCollider;
    [SerializeField] private Animator animator; 
    [SerializeField] private List<EntityStats> statsList;
    [SerializeField] private List<EntityFunctions> functionsList;
    [SerializeField] private List<EntityType> typeList;
    [SerializeField] private EntityState currentState;

    [HideInInspector] public DataLibrary dataLibrary;

    //private IEnumerator stateUpdateCoroutine;
    private bool isActive;

    private void Awake()
    {
        dataLibrary = new DataLibrary();
        for (int iter = 0; iter < statsList.Count; iter++)
        {
            //float valueToAdd = statsList[iter].GetValue();
            //int keyIndex = (int)statsList[iter].GetStatType();
            //dataLibrary.AddFloat(keyIndex, valueToAdd);


            float valueToAdd = statsList[iter].GetValue();
            string keyIndex = statsList[iter].GetStatType().ToString();
            dataLibrary.AddFloat(keyIndex, valueToAdd);
        }
        currentState.Init(this);
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            EntityStateUpdate();
        }
    }

    private void EntityStateUpdate()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    //compare attacker type to the defender type 
    private float CalculateDamageByType(TypeInteractions typeInteraction, float refEntityDamage)
    {
        float damageMultipler = 1f;
        EntityType selfMaterialType = GetMaterialType();
        if (selfMaterialType.GetEntityType() == typeInteraction.strongAgainst.GetEntityType())
            damageMultipler = 2f;
        else if (selfMaterialType.GetEntityType() == typeInteraction.weakAgainst.GetEntityType())
            damageMultipler = 0.5f;

        // apply damage 
        float baseDamage = refEntityDamage;
        float totalDamage = baseDamage * damageMultipler;
        return totalDamage;
    }


    private void TakeDamage(float damage)
    {
        float currentHealth = GetCurrentStatValue(STATSTYPE.HEALTH);
        float newHealth = currentHealth - damage;
        SetCurrentStatValue(STATSTYPE.HEALTH, newHealth);
        //Debug.Log(this.gameObject.name + "'S NEW HEALTH " + newHealth);
    }


    //melee attacks 
    public void HandleDamageFromEntity(Entity attacker)
    {
        TypeInteractions typeInteraction = null;
        if (attacker != null)
            typeInteraction = TypeInteractionMap.instance.GetTypeInteraction(attacker.GetMaterialType());

        if (typeInteraction != null)
        {
            float damageToTake = CalculateDamageByType(typeInteraction, attacker.GetCurrentStatValue(STATSTYPE.DAMAGE));
            TakeDamage(damageToTake);
        }
    }

    //29/9 could be a future problem if there are more than one projectiles acting on the entity but we'll deal with that later 
    //ranged attacks
    public void HandleDamageFromProjectiles()
    {
        TypeInteractions typeInteraction = null;
        Entity refEntity = null;
        //check if it is a projectile with the type 
        if (tagCollider.currentColliding != null)
        {
            RequireParentReference typeData = tagCollider.currentColliding.GetComponent<RequireParentReference>();
            if (typeData != null)
                refEntity = typeData.GetReferenceEntity();

            if (refEntity != null)
                typeInteraction = TypeInteractionMap.instance.GetTypeInteraction(refEntity.GetMaterialType());
        }

        if (typeInteraction != null)
        {
            float damageToTake = CalculateDamageByType(typeInteraction, refEntity.GetCurrentStatValue(STATSTYPE.DAMAGE));
            TakeDamage(damageToTake);
        }
    }


    public void TransitionState(EntityState newState)
    {
        if (newState.name != "RemainState")
        {
            currentState = newState;
            currentState.Init(this);
            //Debug.Log(this.gameObject.name + " ACTIVE STATE " + newState.name);
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////    GETTERS AND SETTERS 

    //look for the values in the active float list not the stats template list 
    public float GetCurrentStatValue(STATSTYPE statType)
    {
        //int keyIndex = (int)statType;
        string keyIndex = statType.ToString();
        return dataLibrary.GetFloat(keyIndex);
    }

    public void SetCurrentStatValue(STATSTYPE statType, float value)
    {
        //int keyIndex = (int)statType;
        string keyIndex = statType.ToString();  
        dataLibrary.SetFloat(keyIndex, value);
    }


    public bool CheckIfThisEntityType(ENTITYTYPE entityType)
    {
        bool isTypeCorrect = false;
        for (int iter = 0; iter < typeList.Count; iter++)
        {
            if (typeList[iter].GetEntityType() == entityType)
            {
                isTypeCorrect = true;
                break;
            }
        }
        return isTypeCorrect;
    }

    //to find the enum and use in the type damage calculations 
    public EntityType GetMaterialType()
    {
        for (int iter = 0; iter < typeList.Count; iter++)
        {
            string enumString = typeList[iter].GetEntityType().ToString();
            if (enumString.StartsWith("D_") || enumString.StartsWith("E_"))
                return typeList[iter];
        }
        return null;
    }


    public void PlayAnimation(ENTITYANIMS anim)
    {
        //check required activation parameter
        AnimatorControllerParameterType paramType = AnimatorControllerParameterType.Bool;
        string enumName = anim.ToString();

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == enumName)
            {
                paramType = param.type;
                break;
            }
        }

        switch (paramType) {

            case AnimatorControllerParameterType.Bool:
                bool currentCondition = animator.GetBool(enumName);
                currentCondition = !currentCondition;
                animator.SetBool(enumName, currentCondition);
                break;
            case AnimatorControllerParameterType.Trigger:
                animator.SetTrigger(enumName);
                break;
        }
    }


    /////////////////////// can consider using in a upgraded version but for now dont use this 
    //public IEnumerator HandleStateUpdates()
    //{
    //    while (true)
    //    {
    //        if (currentState != null)
    //            currentState.UpdateState(this);
    //        else
    //            Debug.Log(this.gameObject.name + " CURRENT STATE NULL");
    //        Debug.Log(this.gameObject.name + " RUNNING");
    //        yield return null;
    //    }
    //}
}
