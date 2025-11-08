using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class HumanEquipmentInfoCard : VisualElement
{
    readonly TemplateContainer templateContainer;

    private HumanEquipmentEntity dataCache;

    private Label title;
    private Label level;
    private Label description;
    private Label attack;
    private Label defense;

    public HumanEquipmentInfoCard() { }

    public HumanEquipmentInfoCard(HumanEquipmentEntity humanEquipmentEntity) {
        dataCache = humanEquipmentEntity;
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/BagUI/components/InfoCard/InfoCard");
        templateContainer = visualTreeAsset.Instantiate();

        var info = humanEquipmentEntity.EquipmentValues;

        title = templateContainer.Q("Title").Q<Label>();
        level = templateContainer.Q("Level").Q<Label>();
        description = templateContainer.Q<VisualElement>("Des").Q<Label>("Value");
        attack = templateContainer.Q<VisualElement>("Attack").Q<Label>("Value");
        defense = templateContainer.Q<VisualElement>("Defense").Q<Label>("Value");

        title.text = info.Name;
        level.text = $"Lv:{info.Level}";
        description.text = info.Description;
        attack.text = info.Attack.ToString();
        defense.text = info.Defense.ToString();

        Add(templateContainer);
    }
}
