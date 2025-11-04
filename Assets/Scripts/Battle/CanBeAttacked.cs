using UnityEngine;

public class CanBeAttacked : MonoBehaviour
{
    private BattleHealthBar healthBarScript;

    private BattleBasicInfos basicInfos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBarScript = gameObject.GetComponentInChildren<BattleHealthBar>();
        if (basicInfos != null)
        {
            UpdateHealthBar(basicInfos.CurrentHealth, basicInfos.MaxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBeAttackedAnimation()
    {
        Debug.Log("PlayBeAttackedAnimation");
    }

    public void UpdateHealthBar(int current, int total) {
        healthBarScript?.UpdateHealthBar(current, total);
    }

    public void SetBasicInfo(BattleBasicInfos battleBasicInfos)
    {
        basicInfos = battleBasicInfos;
    }
}
