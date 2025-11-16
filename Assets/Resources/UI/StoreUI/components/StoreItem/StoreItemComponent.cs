using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class StoreItemComponent : VisualElement
{
    const string ACTIVE_CLASS = "active";

    [UxmlAttribute]
    public string Label
    {
        set
        {
            _label = value;
            labelElm.text = value;
        }
        get
        {
            return _label;
        }
    }
    private string _label;

    [UxmlAttribute]
    public string Value
    {
        set
        {
            _value = value;
            valueElm.text = value;
        }
        get
        {
            return _value;
        }
    }
    private string _value;

    private VisualElement templateContainer;
    private Label labelElm;
    private Label valueElm;

    public HumanEquipmentEntity HumanEquipmentCache;


    public StoreItemComponent()
    {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/StoreUI/components/StoreItem/StoreItem");
        templateContainer = visualTreeAsset.Instantiate();

        labelElm = templateContainer.Q<Label>("Name");
        valueElm = templateContainer.Q<Label>("Price");

        Add(templateContainer);
    }

    public StoreItemComponent(string label, string value) : this()
    {
        Label = label;
        Value = value;
    }

    public StoreItemComponent(HumanEquipmentEntity humanEquipmentEntity) : this() {
        HumanEquipmentCache = humanEquipmentEntity;
        var values = humanEquipmentEntity.EquipmentValues;
        Label = values.Name;
        Value = humanEquipmentEntity.Price.ToString() + 'G';
    }

    public StoreItemComponent SetAcitve(bool isActive)
    {
        if (isActive) {
            templateContainer.AddToClassList(ACTIVE_CLASS);
        }
        else
        {
            templateContainer.RemoveFromClassList(ACTIVE_CLASS);
        }

        return this;
    }
}
