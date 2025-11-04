using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHealthBar : MonoBehaviour
{
    [SerializeField]
    Image HealthBarImg;
    [SerializeField]
    TextMeshProUGUI HealthText;
    [SerializeField]
    float MaxWidth;

    [Header("Debug")]
    [SerializeField, Range(0, 1)]
    float CurrentPercent;

    private RectTransform healthBarRectTransform;

    private void Awake()
    {
        healthBarRectTransform = HealthBarImg.GetComponent<RectTransform>();
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        HealthText.text = $"{currentHealth}/{maxHealth}";
        float percent = (float) Math.Max(0, currentHealth) / maxHealth;
        Debug.Log($"Update health bar {percent}");
        healthBarRectTransform.sizeDelta = new Vector2(MaxWidth * percent, healthBarRectTransform.sizeDelta.y);
    }
}
