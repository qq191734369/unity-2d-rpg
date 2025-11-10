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

    public CharacterItemUI() { }

    public CharacterItemUI(CharacterEntity characterEntity)
    {
        dataCache = characterEntity;
        var info = characterEntity.info;

        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterSelectorUI/CharacterItemUI");
        Texture2D avartar = Resources.Load<Texture2D>($"Sprites/Characters/{info.Name}/{info.Name}-Avatar");
        templateContainer = visualTreeAsset.Instantiate();

        nameLabel = templateContainer.Q<Label>("Name");
        avartarElm = templateContainer.Q("Avartar");

        avartarElm.style.backgroundImage = avartar;

        nameLabel.text = info.Name;

        Add(templateContainer);
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
