using System;
using UnityEngine;

public class CanAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AttackInfo Attack(BattleVisual current)
    {
        BattleVisual target = current.AttackTarget;
        if (target == null )
        {
            Debug.Log($"{current.entity.info.Name} Attack Target is null");
            return null;
        }

        // ≤•∑≈π•ª˜∂Øª≠
        PlayAttackAnimation();
       
        // º∆À„ Ù–‘
        BattleBasicInfos targetInfo = target.entity.info;
        BattleBasicInfos currentInfo = current.entity.info;

        // º∆À„…À∫¶
        int damage = current.entity.Attack - target.entity.Defense;
        // Œ¥∆∆∑¿
        if (damage <= 0)
        {
            damage = 1;
        }

        targetInfo.CurrentHealth -= damage;
        Debug.Log($"currenthealth {target.entity.CurrentHealth}, max health {target.entity.MaxHealth}");
        // ≤•∑≈ ‹ª˜∂Øª≠
        target.instance.GetComponent<CanBeAttacked>()?.PlayBeAttackedAnimation();
        target.instance.GetComponent<CanBeAttacked>()?.UpdateHealthBar(target.entity.CurrentHealth, target.entity.MaxHealth);

        return new AttackInfo
        {
            CurrentName = currentInfo.Name,
            TargetName = targetInfo.Name,
            Damage = damage
        };
    }

    private void PlayAttackAnimation()
    {
        Debug.Log("PlayAttackAnimation");
    }
}

public class AttackInfo
{
    public string TargetName;
    public int Damage;
    public string CurrentName;
}

