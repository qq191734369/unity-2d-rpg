using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ChatOptions : VisualElement
{
    static string ACTIVE_CLASS = "active";

    public bool Disabled = false;

    private List<VisualElement> optionsItems = new List<VisualElement>();

    private List<ChatOption> chatOptions;

    private int activeIndex = 0;

    public ChatOptions() { }

    public ChatOptions(List<ChatOption> options) {
        chatOptions = options;

        VisualElement container = BuildContainer();
        
        foreach (ChatOption op in options)
        {
            VisualElement chatOptionElement = BuildOptionItemElement(op);
            container.Add(chatOptionElement);

            // 缓存元素，便于后续设置class
            optionsItems.Add(chatOptionElement);
        }
        optionsItems[activeIndex].AddToClassList(ACTIVE_CLASS);
        Add(container);
    }

    public void SelectPre() {
        int index = (activeIndex + 1) % optionsItems.Count;
        optionsItems[activeIndex].RemoveFromClassList(ACTIVE_CLASS);

        SetActiveItem(index);
    }

    public void SelectNext() {
        int tempIndex = activeIndex - 1;
        int index = tempIndex < 0 ? optionsItems.Count - 1 : tempIndex;
        optionsItems[activeIndex].RemoveFromClassList(ACTIVE_CLASS);

        SetActiveItem(index);
    }

    public ChatOption GetActiveOption()
    {
        return chatOptions[activeIndex];
    }

    private void SetActiveItem(int idx) {
        activeIndex = idx;
        optionsItems[activeIndex].AddToClassList(ACTIVE_CLASS);
    }

    private VisualElement BuildContainer()
    {
        VisualElement container = new VisualElement();
        container.name = "ChatOptions";
        container.AddToClassList("options");

        return container;
    }

    private VisualElement BuildOptionItemElement(ChatOption op) {
        VisualElement chatOptionItem  = new VisualElement();
        Label label = new Label();

        chatOptionItem.name = "ChatOptionItem";
        chatOptionItem.AddToClassList("optionItem");

        chatOptionItem.Add(label);
        label.text = op.Text;

        return chatOptionItem;
    }
}
