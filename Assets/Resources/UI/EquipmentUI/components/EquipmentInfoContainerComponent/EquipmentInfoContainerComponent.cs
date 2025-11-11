using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class EquipmentInfoContainerComponent : VisualElement
{

    private VisualElement templateContainer;
    private VisualElement container;

    public EquipmentInfoContainerComponent()
    {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/EquipmentUI/components/EquipmentInfoContainerComponent/EquipmentInfoSection");
        templateContainer = visualTreeAsset.Instantiate();
        container = templateContainer.Q<VisualElement>("EquipmentInfoSection");

        Add(templateContainer);
    }

    public EquipmentInfoContainerComponent(HumanEquipmentEntity equipmentEntity) : this()
    {
        if (equipmentEntity == null) {
            return;
        }
        var info = equipmentEntity.EquipmentValues;

        container.Clear();

        container.Add(new EquipmentInfoLineComponent("攻击力:", info.Attack.ToString()));
        container.Add(new EquipmentInfoLineComponent("防御力:", info.Defense.ToString()));
        container.Add(new EquipmentInfoLineComponent("速度:", info.Speed.ToString()));
        container.Add(new EquipmentInfoLineComponent("生命值:", info.MaxHealth.ToString()));
    }
}
