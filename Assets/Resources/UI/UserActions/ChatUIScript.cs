using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatUIScript : MonoBehaviour
{
    const string TEXT_KEY = "Text";

    public bool isActive = false;
    public bool isSelectingOptions = false;

    public System.Action<ChatOption> OnConfirm;

    UIDocument uiDocument;
    VisualElement Container;

    private ChatOptions optionsElement;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        Container = uiDocument.rootVisualElement.Q("Bg");
    }

    private void Update()
    {
        DetectOptionChange();
    }

    public ChatUIScript Show()
    {
        Container.style.display = DisplayStyle.Flex;
        isActive = true;
        return this;
    }

    public ChatUIScript Hide() {
        Container.style.display = DisplayStyle.None;
        isActive = false;
        return this;
    }

    public ChatUIScript UpdateText(string text) {
        Label label = uiDocument.rootVisualElement.Q<Label>(TEXT_KEY);
        label.text = text;

        return this;
    }

    public ChatUIScript UpdateText(string text, List<ChatOption> ops)
    {
        UpdateText(text);

        ShowOptions(ops);

        return this;
    }

    public ChatUIScript ShowOptions(List<ChatOption> ops)
    {
        if (optionsElement != null)
        {
            optionsElement.RemoveFromHierarchy();
        }

        if (ops != null)
        {
            CreateOptionBtns(ops);
            isSelectingOptions = true;
            Debug.Log($"isSelectingOptions {isSelectingOptions}");
        }

        return this;
    }

    public ChatUIScript HideOptions()
    {
        optionsElement.RemoveFromHierarchy();
        isSelectingOptions = false;
        Debug.Log($"isSelectingOptions {isSelectingOptions}");
        return this;
    }

    private void CreateOptionBtns(List<ChatOption> ops) {
        optionsElement = new ChatOptions(ops);
        Container.Add(optionsElement);
    }

    private async void DetectOptionChange()
    {
        // ±‹√‚±®¥Ì
        if (optionsElement == null || !isActive || optionsElement.Disabled || ChatSystem.isSleep)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("OnConfirm?.Invoke");
            OnConfirm?.Invoke(optionsElement.GetActiveOption());
            await Task.Delay(100);
            HideOptions();
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            optionsElement.SelectPre();
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            optionsElement.SelectNext();
        }
    }
}
