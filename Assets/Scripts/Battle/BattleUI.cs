using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BattleUI : MonoBehaviour
{
    const string BTN_ACTIVE_CLASS = "btnActive";

    public System.Action<BattleVisual> ActionSelectEnemy;
    public System.Action ActionRun;

    // 操作按钮
    private List<VisualElement> actionBtns = new List<VisualElement>();
    private UIDocument document;
    private VisualElement rootElement;
    private VisualElement battleInfoPanel;
    private VisualElement enemySelectPanel;
    private VisualElement battlerLabel;
    // 激活的按钮
    private int activeBtnIndex;
    // 敌人列表，UI显示时传入
    private List<BattleVisual> enemyBattleVisuals = new List<BattleVisual>();
    private List<UIEnemyItem> enemyUIList = new List<UIEnemyItem>();

    // 激活的敌人按钮
    private int activeEnemyIndex;


    private void Awake()
    {
        document = GetComponent<UIDocument>();
        rootElement = document.rootVisualElement;
        battleInfoPanel = rootElement.Q("BattleInfo");
        enemySelectPanel = rootElement.Q("EnemyList");
        battlerLabel = rootElement.Q("BattlerInfo");
    }

    private void Update()
    {
        DetectBtnSelections();
    }

    public BattleUI ShowBattlerLabel(string text)
    {
        battlerLabel.style.display = DisplayStyle.Flex;
        battlerLabel.Q<Label>().text = text;
        return this;
    }

    public BattleUI HideBattlerLabel()
    {
        battlerLabel.style.display = DisplayStyle.None;
        battlerLabel.Q<Label>().text = "";
        return this;
    }

    public BattleUI ShowIU(List<BattleVisual> enemyBattleVisuals) {
        this.enemyBattleVisuals = enemyBattleVisuals;
        InitActionList();
        ShowSelectActionPanel();

        return this;
    }

    private void InitActionList()
    {
        actionBtns = rootElement.Query<VisualElement>(classes: new[] { "btn" }).ToList();
        Debug.Log("Battle UI - InitActionList" + $"actionBtns lenth: {actionBtns.Count}");
        
        if (actionBtns.Count == 0)
        {
            return;
        }

        actionBtns[0].AddToClassList(BTN_ACTIVE_CLASS);
        activeBtnIndex = 0;
    }

    public void ClearBattleMessage()
    {
        battleInfoPanel.Q<Label>("BattleInfoText").text = "";
    }

    public void AppendBattleMessage(string msg)
    {
        battleInfoPanel.style.display = DisplayStyle.Flex;
        enemySelectPanel.style.display = DisplayStyle.None;
        Label label = battleInfoPanel.Q<Label>("BattleInfoText");
        string currentText = label.text;
        label.text = $"{currentText} \n" + msg;

        ScrollView scrollView = battleInfoPanel.Q<ScrollView>();
        // 设置垂直滚动位置为最大值
        scrollView.scrollOffset = new Vector2(0, scrollView.contentContainer.worldBound.height);
    }

    private void ShowSelectActionPanel()
    {
        ClearBattleMessage();
        battleInfoPanel.style.display = DisplayStyle.Flex;
        enemySelectPanel.style.display = DisplayStyle.None;
    }

    private void ShowSelectEnemyPanel()
    {
        Debug.Log("method called ShowSelectEnemyPanel");
        battleInfoPanel.style.display = DisplayStyle.None;
        enemySelectPanel.style.display = DisplayStyle.Flex;

        // 控制敌人UI显示
        enemySelectPanel.Clear();
        enemyUIList.Clear();
        for (int i = 0; i < enemyBattleVisuals.Count; i++)
        {
            UIEnemyItem uIEnemyItem = new UIEnemyItem(enemyBattleVisuals[i]);
            enemyUIList.Add(uIEnemyItem);
            enemySelectPanel.Add(uIEnemyItem);
        }
        Debug.Log(enemySelectPanel);
        Debug.Log($"UIEnemyItem number: {enemyUIList.Count}");

        activeEnemyIndex = 0;
        enemyUIList[activeEnemyIndex].SetActiveState(true);
    }

    private void UpdateActionBtnVisual(int preActiveIndex)
    {
        if (preActiveIndex == activeBtnIndex)
        {
            return;
        }
        // 更新按钮样式
        actionBtns[preActiveIndex].RemoveFromClassList(BTN_ACTIVE_CLASS);
        actionBtns[activeBtnIndex].AddToClassList(BTN_ACTIVE_CLASS);
    }

    private void UpdateSelectEnemyBtnVisual(int preActiveEnemyIndex)
    {
        if (preActiveEnemyIndex == activeEnemyIndex)
        {
            return;
        }
        // 更新按钮状态
        enemyUIList[activeEnemyIndex].SetActiveState(true);
        enemyUIList[preActiveEnemyIndex].SetActiveState(false);
    }

    private void DetectBtnSelections()
    {
        // 选择角色行为界面
        if (BattleManager.CurrentBattleStatus == BattleManager.BattleActionStatus.SelectAction)
        {
            int preActiveIndex = activeBtnIndex;
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                int index = activeBtnIndex - 2;
                activeBtnIndex = index < 0 ? 0 : index;
                UpdateActionBtnVisual(preActiveIndex);

            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                int index = activeBtnIndex + 2;
                activeBtnIndex = index >= actionBtns.Count ? actionBtns.Count - 1 : index;
                UpdateActionBtnVisual(preActiveIndex);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                int index = activeBtnIndex - 1;
                activeBtnIndex = index < 0 ? 0 : index;
                UpdateActionBtnVisual(preActiveIndex);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                int index = activeBtnIndex + 1;
                activeBtnIndex = index >= actionBtns.Count ? actionBtns.Count - 1 : index;
                UpdateActionBtnVisual(preActiveIndex);
            }


            // 确定键
            if (Input.GetKeyUp(KeyCode.X))
            {
                Debug.Log($"press X, current active btn: {actionBtns[activeBtnIndex].name}");

                switch (actionBtns[activeBtnIndex].name)
                {
                    case "Attack":
                        BattleManager.CurrentBattleStatus = BattleManager.BattleActionStatus.SelectEnemy;
                        ShowSelectEnemyPanel();
                        break;
                    case "Run":
                        ActionRun?.Invoke();
                        break;
                    default:
                        Debug.Log("No action matched");
                        break;
                }
            }
        }
        // 选择敌人界面
        else if (BattleManager.CurrentBattleStatus == BattleManager.BattleActionStatus.SelectEnemy)
        {
            FreshEnemyList();
        }
    }

    private void FreshEnemyList()
    {
        // 选择敌人
        int preActiveEnemyIndex = activeEnemyIndex;
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            int index = activeEnemyIndex - 1;
            activeEnemyIndex = index < 0 ? 0 : index;
            UpdateSelectEnemyBtnVisual(preActiveEnemyIndex);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            int index = activeEnemyIndex + 1;
            activeEnemyIndex = index >= enemyUIList.Count ? enemyUIList.Count - 1 : index;
            UpdateSelectEnemyBtnVisual(preActiveEnemyIndex);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            int index = activeEnemyIndex - 4;
            activeEnemyIndex = index < 0 ? 0 : index;
            UpdateSelectEnemyBtnVisual(preActiveEnemyIndex);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            int index = activeEnemyIndex + 4;
            activeEnemyIndex = index >= enemyUIList.Count ? enemyUIList.Count - 1 : index;
            UpdateSelectEnemyBtnVisual(preActiveEnemyIndex);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            BattleManager.CurrentBattleStatus = BattleManager.BattleActionStatus.SelectAction;
            ShowSelectActionPanel();
            // 传出选择敌人
            ActionSelectEnemy?.Invoke(enemyBattleVisuals[activeEnemyIndex]);
        }
    }
}
