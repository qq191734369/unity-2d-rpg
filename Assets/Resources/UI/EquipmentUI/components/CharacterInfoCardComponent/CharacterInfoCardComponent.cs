using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CharacterInfoCardComponent : VisualElement
{

    private VisualElement templateContainer;
    private VisualElement characterInfoSlot;
    private Label nameLabel;

    public CharacterInfoCardComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/EquipmentUI/components/CharacterInfoCardComponent/CharacterInfoContainer");
        templateContainer = visualTreeAsset.Instantiate();
        characterInfoSlot = templateContainer.Q<VisualElement>("CharacterInfo");
        nameLabel = templateContainer.Q<Label>("Name");

        Add(templateContainer);
    }

    public CharacterInfoCardComponent(CharacterEntity characterEntity) : this() {
        var info = characterEntity.info;
        nameLabel.text = info.Name;

        characterInfoSlot.Clear();

        characterInfoSlot.Add(new CharacterInfoItemComponent("攻击力:", characterEntity.Attack.ToString()));
        characterInfoSlot.Add(new CharacterInfoItemComponent("防御力:", characterEntity.Defense.ToString()));
        characterInfoSlot.Add(new CharacterInfoItemComponent("速度:", characterEntity.Speed.ToString()));
        characterInfoSlot.Add(new CharacterInfoItemComponent("生命值:", $"{characterEntity.CurrentHealth}/{characterEntity.MaxHealth}"));
    }
}
