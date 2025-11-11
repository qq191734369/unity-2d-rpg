using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class EquipmentInfoLineComponent : VisualElement
{
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

    public EquipmentInfoLineComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/EquipmentUI/components/EquipmentInfoLineComponent/EquipmentInfoLine");
        templateContainer = visualTreeAsset.Instantiate();

        labelElm = templateContainer.Q<Label>("Label");
        valueElm = templateContainer.Q<Label>("Value");

        Add(templateContainer);
    }

    public EquipmentInfoLineComponent(string label, string value) : this()
    {
        Label = label;
        Value = value;
    }
}
