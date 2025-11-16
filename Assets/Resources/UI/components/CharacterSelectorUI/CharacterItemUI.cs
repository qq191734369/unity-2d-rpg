using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CharacterItemUI : VisualElement
{
    const string ACTIVE_CLASS = "active";

    public CharacterEntity dataCache;

    private VisualElement templateContainer;
    private Label nameLabel;
    private VisualElement avartarElm;

    public CharacterItemUI() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterSelectorUI/CharacterItemUI");
        templateContainer = visualTreeAsset.Instantiate();

        nameLabel = templateContainer.Q<Label>("Name");
        avartarElm = templateContainer.Q("Avartar");

        Add(templateContainer);
    }

    public CharacterItemUI(CharacterEntity characterEntity): this()
    {
        dataCache = characterEntity;
        var info = characterEntity.info;

        Texture2D avartar = ResourceUtil.GetCharacterAvartar<Texture2D>(characterEntity);

        avartarElm.style.backgroundImage = avartar;
        nameLabel.text = info.Name;
    }

    public CharacterItemUI(string text) : this()
    {
        nameLabel.text = text;
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            templateContainer.AddToClassList(ACTIVE_CLASS);
        } else
        {
            templateContainer.RemoveFromClassList(ACTIVE_CLASS);
        }
    }
}
