using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CommonCharacterInfoCardComponent : VisualElement
{

    VisualElement templateContainer;
    Label nameElm;
    Label levelElm;
    VisualElement contentElm;

    public CommonCharacterInfoCardComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterInfoCard/CharacterInfoCard");
        templateContainer = visualTreeAsset.Instantiate();

        nameElm = templateContainer.Q<Label>("Name");
        levelElm = templateContainer.Q<Label>("Level");
        contentElm = templateContainer.Q<VisualElement>("Content");

        Add(templateContainer);
    }

    public CommonCharacterInfoCardComponent(CharacterEntity characterEntity, HumanEquipmentEntity humanEquipmentEntity = null) : this()
    {
        var info = characterEntity.info;
        nameElm.text = info.Name;
        levelElm.text = $"Lv: {info.Level}";

        var previewEntity = characterEntity.PreviewInfos(humanEquipmentEntity);

        contentElm.Clear();
        contentElm.Add(new CommonCharacterInfoCardLineComponent("攻击力", characterEntity.Attack.ToString(), previewEntity.Attack.ToString()));
        contentElm.Add(new CommonCharacterInfoCardLineComponent("防御力", characterEntity.Defense.ToString(), previewEntity.Defense.ToString()));
        contentElm.Add(new CommonCharacterInfoCardLineComponent("速度", characterEntity.Speed.ToString(), previewEntity.Speed.ToString()));
        contentElm.Add(new CommonCharacterInfoCardLineComponent("生命值", characterEntity.MaxHealth.ToString(), previewEntity.MaxHealth.ToString()));
    }
}
