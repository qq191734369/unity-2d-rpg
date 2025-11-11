using UnityEngine;

public class CanBeAttacked : MonoBehaviour
{
    private BattleHealthBar healthBarScript;

    private CharacterEntity characterEntity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBarScript = gameObject.GetComponentInChildren<BattleHealthBar>();
        if (characterEntity != null)
        {
            UpdateHealthBar(characterEntity.info.CurrentHealth, characterEntity.MaxHealth);
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

    public void SetBasicInfo(CharacterEntity entity)
    {
        characterEntity = entity;
    }
}
