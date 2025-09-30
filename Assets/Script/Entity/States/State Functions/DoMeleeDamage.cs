using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "DoMeleeDamage", menuName = "Scriptable Objects/DoMeleeDamage")]
public class DoMeleeDamage : EntityFunctions
{
    [SerializeField] private string layerName;
    [SerializeField] private float attackRange;
    [SerializeField] private Vector2 attackDirection;
    [SerializeField] private float attackCooldown;



    public override IEnumerator ExcuteCoroutine(Entity entity = null)
    {

        while (true)
        {
            Entity target = null;
            bool validTarget = false;
            RaycastHit2D hit = Physics2D.Raycast(entity.gameObject.transform.position, attackDirection, attackRange, LayerMask.GetMask(layerName));
            if (hit.collider != null)
            {
                target = hit.collider.gameObject.GetComponent<Entity>();
                if (target != null)
                    validTarget = true;
            }


            //let entity handle this themselves
            if (validTarget)
                target.HandleDamageFromEntity(entity);
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public override void CopyData(EntityFunctions reference)
    {
        if (reference is DoMeleeDamage MeleeDamage)
        {
            layerName = MeleeDamage.layerName; 
            attackRange = MeleeDamage.attackRange; 
            attackDirection = MeleeDamage.attackDirection; 
            attackCooldown = MeleeDamage.attackCooldown; 
        }
    }

}