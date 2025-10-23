using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class UIEnemyItem : VisualElement
{
    readonly TemplateContainer templateContainer;

    public UIEnemyItem() {}

    public UIEnemyItem(BattleVisual enemyBattleVisual) : this()
    {
        Debug.Log("Create UIEnemyItem with param");
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/BattleUI/template/EnemyItem");
        templateContainer = visualTreeAsset.Instantiate();
        BattleBasicInfos info = enemyBattleVisual.info;
        templateContainer.Q<Label>().text = $"Lv {info.Level} : {info.Name}";

        Add(templateContainer);
    }

    public void SetActiveState(bool active)
    {
        if (active)
        {
            templateContainer.AddToClassList("enemyItemActive");
        }
        else
        {
            templateContainer.RemoveFromClassList("enemyItemActive");
        }
    }
}
