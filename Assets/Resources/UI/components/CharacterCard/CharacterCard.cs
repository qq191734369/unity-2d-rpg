using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CharacterCard : VisualElement
{
    readonly TemplateContainer templateContainer;

    Label Name;
    VisualElement CurrentHealthBar;
    Label HealthNum;
    VisualElement Avartar;
    Label Level;
    Label NextExp;
    Label CurrentExp;

    public CharacterCard() { }

    public CharacterCard(CharacterEntity characterEntity)
    {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterCard/CharacterCard");
        templateContainer = visualTreeAsset.Instantiate();

        Name = templateContainer.Q<Label>("CaracterName");
        HealthNum = templateContainer.Q<Label>("HealNum");
        CurrentHealthBar = templateContainer.Q<VisualElement>("CurrentHealth");
        Avartar = templateContainer.Q<VisualElement>("Avartar");
        Level = templateContainer.Q<Label>("Level");
        NextExp = templateContainer.Q<Label>("NextExp");
        CurrentExp = templateContainer.Q<Label>("CurrentExp");

        UpdateData(characterEntity);

        Add(templateContainer);
    }

    public CharacterCard UpdateData(CharacterEntity characterEntity)
    {
        BattleBasicInfos info = characterEntity.info;
        var name = info.Name;

        Name.text = name;
        Level.text = $"等级: {info.Level}";
        HealthNum.text = $"{info.CurrentHealth}/{info.MaxHealth}";
        CurrentHealthBar.style.width = Length.Percent((float)info.CurrentHealth / info.MaxHealth * 100);
        Texture2D texture2D = Resources.Load<Texture2D>($"Sprites/Characters/{name}/{name}-Avatar");
        Avartar.style.backgroundImage = new StyleBackground(texture2D);

        // 计算下一级经验
        long nextExp = LevelManager.GetExpToLevelUp(characterEntity);
        NextExp.text = $"下一级还需exp: {nextExp}";
        CurrentExp.text = $"当前exp: {info.CurrentExp}";

        return this;
    }
}
