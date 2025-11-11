using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CharacterInfoItemComponent : VisualElement
{
    [UxmlAttribute]
    public string Label
    {
        get
        {
            return _label;
        }
        set
        {
            _label = value;
            labelElm.text = value;
        }
    }
    private string _label;

    [UxmlAttribute]
    public string Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            valueElm.text = value;
        }
    }
    private string _value;

    private VisualElement templateContaienr;
    private Label labelElm;
    private Label valueElm;

    public CharacterInfoItemComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/EquipmentUI/components/CharacterInfoItemComponent/CharacterInfoItem");
        templateContaienr = visualTreeAsset.Instantiate();

        labelElm = templateContaienr.Q<Label>("Label");
        valueElm = templateContaienr.Q<Label>("Value");

        Add(templateContaienr);
    }

    public CharacterInfoItemComponent(string label, string value): this() {
        Label = label;
        Value = value;
    }
}
