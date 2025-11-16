using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CommonCharacterInfoCardLineComponent : VisualElement
{
    const string DOWN_CLASS = "down";
    const string UP_CLASS = "up";

    [UxmlAttribute]
    public string Label
    {
        get
        {
            return _label;
        }
        set
        {
            valueNameElm.text = value;
            _label = value;
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
            currentValueElm.text = value;
            _value = value;
        }
    }
    private string _value;

    [UxmlAttribute]
    public string TargetValue
    {
        get
        {
            return _targetValue;
        }
        set
        {
            targetValueElm.text = value;
            _targetValue = value;
        }
    }
    private string _targetValue;

    VisualElement templateContainer;
    Label valueNameElm;
    Label currentValueElm;
    Label iconElm;
    Label targetValueElm;

    public CommonCharacterInfoCardLineComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterInfoCard/CharacterInfoLine");
        templateContainer = visualTreeAsset.Instantiate();

        valueNameElm = templateContainer.Q<Label>("ValueName");
        currentValueElm = templateContainer.Q<Label>("CurrentValue");
        iconElm = templateContainer.Q<Label>("Icon");
        targetValueElm = templateContainer.Q<Label>("TargetValue");

        Add(templateContainer);
    }

    public CommonCharacterInfoCardLineComponent(string label, string value, string targetValue = null) : this()
    {
        Label =  label;
        Value = value;
        TargetValue = targetValue;

        if (targetValue == null)
        {
            iconElm.style.display = DisplayStyle.None;
        }
        else
        {
            int currentNum = int.Parse(value);
            int targetNum = int.Parse(targetValue);
            int delta = currentNum - targetNum;

            if (delta < 0)
            {
                targetValueElm.AddToClassList(UP_CLASS);
            } else if (delta > 0)
            {
                targetValueElm.AddToClassList(DOWN_CLASS);
            }
        }
    }
}
