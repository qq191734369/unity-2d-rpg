using UnityEngine;

public class CanAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AttackInfo Attack(BattleVisual current)
    {
        BattleVisual target = current.AttackTarget;
        if (target == null )
        {
            Debug.Log($"{(current.info as BattleBasicInfos).Name} Attack Target is null");
            return null;
        }

        // 播放攻击动画
        PlayAttackAnimation();
        // 播放受击动画
        target.instance.GetComponent<CanBeAttacked>()?.PlayBeAttackedAnimation();
        // 计算属性
        BattleBasicInfos targetInfo = target.info as BattleBasicInfos;
        BattleBasicInfos currentInfo = current.info as BattleBasicInfos;

        targetInfo.CurrentHealth -= currentInfo.Attack;

        return new AttackInfo
        {
            CurrentName = currentInfo.Name,
            TargetName = targetInfo.Name,
            Damage = currentInfo.Attack
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

