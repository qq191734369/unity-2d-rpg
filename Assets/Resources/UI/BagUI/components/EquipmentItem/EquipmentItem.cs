using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class EquipmentItem : VisualElement
{
    const string ACTIVE_CLASS = "active";

    public event System.Action<HumanEquipmentEntity> OnHover;

    public HumanEquipmentEntity dataCache;

    readonly TemplateContainer templateContainer;

    private Label name;

    private Label level;


    public EquipmentItem() { }

    public EquipmentItem(HumanEquipmentEntity humanEquipmentEntity) {
        dataCache = humanEquipmentEntity;
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/BagUI/components/EquipmentItem/EquipItem");
        templateContainer = visualTreeAsset.Instantiate();

        name = templateContainer.Q<Label>("Name");
        level = templateContainer.Q<Label>("Level");

        var info = humanEquipmentEntity.EquipmentValues;
        
        name.text = info.Name;
        level.text = $"Lv:{info.Level.ToString()}";

        Add(templateContainer);

        templateContainer.RegisterCallback<MouseEnterEvent>(HandleHover);
        templateContainer.RegisterCallback<MouseLeaveEvent>(HandleHoverOver);
    }

    private void HandleHover(MouseEnterEvent e) {
        OnHover?.Invoke(dataCache);
    }

    private void HandleHoverOver(MouseLeaveEvent e) {
        OnHover?.Invoke(null);
    }

    public EquipmentItem SetActive(bool d)
    {
        if (d)
        {
            templateContainer.AddToClassList(ACTIVE_CLASS);
        } else
        {
            templateContainer.RemoveFromClassList(ACTIVE_CLASS);
        }

        return this;
    }
}
