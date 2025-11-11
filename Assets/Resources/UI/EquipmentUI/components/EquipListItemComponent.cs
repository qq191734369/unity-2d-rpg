using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class EquipListItemComponent : VisualElement
{
    const string ACTIVE_CLASS = "activeItem";

    // 公共属性
    [UxmlAttribute]
    public string Tag {
        get
        {
            return _tag;
        }
        set
        {
            _tag = value;
            if (tagElm != null)
            {
                tagElm.text = value;
            }
        }
    }
    [UxmlAttribute]
    public string Name {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            if (nameElm != null)
            {
                nameElm.text = value;
            }
        }
    }

    [UxmlAttribute]
    public HumanEquipmentEntity.Category HumanEquipCategory
    {
        get
        {
            return _humanCategory;
        }

        set {
            _humanCategory = value;
        }
    }

    public HumanEquipmentEntity dataCache;

    private TemplateContainer templateContainer;

    private HumanEquipmentEntity.Category _humanCategory;

    private VisualElement Container1;
    private string _tag;
    private string _name;
    private Label tagElm;
    private Label nameElm;

    private VisualElement Container2;
    private Label bagItemNameElm;
    private Label levelElm;
    // 定义 USS 类名
    public EquipListItemComponent() {
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/EquipmentUI/components/EquipListItemComponent");
        templateContainer = visualTreeAsset.Instantiate();

        Container1 = templateContainer.Q("EquipListItemComponent");
        tagElm = Container1.Q<Label>("Tag");
        nameElm = Container1.Q<Label>("Name");

        tagElm.text = Tag;
        nameElm.text = Name;

        Container2 = templateContainer.Q("BagListComponent");
        bagItemNameElm = Container2.Q<Label>("Name");
        levelElm = Container2.Q<Label>("Level");
        Container2.style.display = DisplayStyle.None;

        Add(templateContainer);
    }

    public EquipListItemComponent(HumanEquipmentEntity humanEquipmentEntity, string tag, HumanEquipmentEntity.Category category) : this()
    {
        HumanEquipCategory = category;
        dataCache = humanEquipmentEntity;
        tagElm.text = tag;

        if (humanEquipmentEntity == null) {
            return;
        }
        var info = humanEquipmentEntity.EquipmentValues;
        nameElm.text = info.Name;
        _humanCategory = humanEquipmentEntity.CategoryType;
    }

    public EquipListItemComponent(HumanEquipmentEntity humanEquipmentEntity): this()
    {
        dataCache = humanEquipmentEntity;
        HumanEquipCategory = humanEquipmentEntity.CategoryType;
        
        Container2.style.display = DisplayStyle.Flex;
        Container1.style.display = DisplayStyle.None;

        var info = humanEquipmentEntity.EquipmentValues;
        bagItemNameElm.text = info.Name;
        levelElm.text = $"Lv:{info.Level}";
    }

    public EquipListItemComponent SetActive(bool active) {
        if (active) {
            templateContainer.AddToClassList(ACTIVE_CLASS);
        } else
        {
            templateContainer.RemoveFromClassList(ACTIVE_CLASS);
        }

        return this;
    }
}
