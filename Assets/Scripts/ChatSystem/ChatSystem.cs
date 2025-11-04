using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{
    public static bool isSleep = false;

    const int DELAY_TIME = 200;
    public enum ChatStatus
    {
        None,
        InProcess,
        Done
    }

    public System.Action OnChatDone;
    public System.Action<CharacterEntity> OnJoinParty;

    public bool IsInConversasion
    {
        get
        {
            return status == ChatStatus.InProcess || isSleep;
        }
    }

    private GameManager gameManager;
    private ChatUIScript chatUIScript;
    private DataManager dataManager;

    private InteractableObject interactableObject;
    private ChatSection currentChatSection;

    private ChatStatus status = ChatStatus.None;
    private int contentIndex;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        chatUIScript = gameManager.ChatUIDocument.GetComponent<ChatUIScript>();
        dataManager = GetComponent<DataManager>();
    }

    private void Update()
    {
        DetectBtnPress();
    }

    private void DetectBtnPress()
    {
        if (
            Input.GetKeyUp(KeyCode.X)
            // 在对话中
            && status == ChatStatus.InProcess
            // 不在选择动作中
            && !chatUIScript.isSelectingOptions
            // 非睡眠
            && !isSleep
        )
        {
            HandleNextChatContent();
        }
    }

    public void StartChat(InteractableObject obj)
    {
        interactableObject = obj;
        ChatSection section = obj.chatSection;
        if (!section.isEmpty())
        {
            // 开始对话
            status = ChatStatus.InProcess;
            currentChatSection = section;
            ShowCurrentSection();
        }
    }

    public void ResetChat()
    {
        interactableObject = null;
        status = default;
        contentIndex = 0;
        currentChatSection = null;
        chatUIScript.Hide();
        Debug.Log($"ResetChat,currentChatSection {currentChatSection}");
    }

    // 进行当前对话片段展示
    private void ShowCurrentSection()
    {
        contentIndex = 0;
        ShowCurrentContent();
        Debug.Log($"Show current chat section");
    }

    private async Task WaitForSeconds()
    {
        isSleep = true;
        await Task.Delay(DELAY_TIME);
        isSleep = false;
    }

    // 对话内容展示
    private async void ShowCurrentContent()
    {
        List<ChatContent> contents = currentChatSection.Contents;
        ChatContent currentContent = contents[contentIndex];
        chatUIScript.Show().UpdateText(currentContent.Text);
        Debug.Log($"Show current chat content, index: {contentIndex}");
        // 短暂停止响应交互
        await WaitForSeconds();
    }

    // 按键后，展示下一个对话内容
    private async void HandleNextChatContent()
    {
        List<ChatContent> contents = currentChatSection.Contents;
        int nextContentIndex = contentIndex + 1;
        // 对话内容未播完 继续下一段内容
        if (nextContentIndex < contents.Count)
        {
            contentIndex++;
            ShowCurrentContent();
        }
        else
        {
            // 对话内容结束, 决定后续动作
            // 1. 是否有Option选项 展示Options
            List<ChatOption> chatOptions = currentChatSection.ChatOptions;
            if (chatOptions != null && chatOptions.Count > 0)
            {
                // 短暂停止响应交互
                await WaitForSeconds();

                chatUIScript.ShowOptions(chatOptions);
                chatUIScript.OnConfirm += HandleOptionConfirm;
                return;
            }
            // 2. 是否有Next
            ChatSection next = currentChatSection.Next;
            if (next != null)
            {
                currentChatSection = next;
                // 展示对话内容
                ShowCurrentSection();
                return;
            }

            // 4. 直接结束
            EmitChatDone();
        }
    }

    private void EmitChatDone()
    {
        status = ChatStatus.Done;
        ResetChat();
        OnChatDone?.Invoke();
    }

    private void ProcessBattle(ChatOption op)
    {
        List<ChatBattleConfig> chatBattleConfig = op.ChatBattleConfig;
        if (chatBattleConfig == null)
        {
            Debug.Log("No Battle params found");
            return;
        }

        SceneParams sceneParams = new SceneParams();
        sceneParams.EnemyList = new List<SceneEnemyInfo>();

        for (int i = 0; i < chatBattleConfig.Count; i++)
        {
            string name = chatBattleConfig[i].MosterName;
            int[] levelRange = chatBattleConfig[i].LevelRange;
            int level = levelRange[Random.Range(0, levelRange.Length)];
            sceneParams.EnemyList.Add(new SceneEnemyInfo
            {
                Name = name,
                Level = level,
            });
        }

        GameManager.SavePosiiton();
        SceneLoader.LoadBattleScene(sceneParams);
    }

    private void HandleOptionConfirm(ChatOption op)
    {
        chatUIScript.OnConfirm -= HandleOptionConfirm;
        ChatOption.Action action = op.ActionWhenChatOver;

        switch (action)
        {
            case ChatOption.Action.Battle:
                Debug.Log("Start Battle");
                ProcessBattle(op);
                break;
            case ChatOption.Action.Join:
                Debug.Log("Join party");
                OnJoinParty?.Invoke(interactableObject.CharacterInfo);
                break;
            case ChatOption.Action.Love:
                Debug.Log("Increase Love");
                break;
            case ChatOption.Action.None:
                Debug.Log("None action");
                break;
            default:
                break;
        }


        if (op.Next == null || op.Next.isEmpty())
        {
            EmitChatDone();
        }
        else
        {
            currentChatSection = op.Next;
            ShowCurrentSection();
        }
    }
}

[System.Serializable]
public class ChatContent
{
    public string Text;
}

[System.Serializable]
public class ChatBattleConfig
{
    public string MosterName;
    public int[] LevelRange;
}

[System.Serializable]
public class ChatLoveConfig
{
    public enum LoveType
    {
        Increase,
        Decrease
    }
    public string[] Targets;
    public LoveType Type;
}

[System.Serializable]
public class ChatOption
{
    public enum Action
    {
        None,
        // 战斗
        Battle,
        // 好感
        Love,
        // 入队
        Join
    }

    public int Id;
    // 选项内容
    public string Text;
    // 选择后动作
    public Action ActionWhenChatOver = Action.None;

    public List<ChatBattleConfig> ChatBattleConfig;

    public ChatLoveConfig LoveConfig;

    public ChatSection Next;
}

// 单向链表数据结构
[System.Serializable]
public class ChatSection
{
    public int Id;
    public string Name;
    // 下一段对话
    public ChatSection Next;
    // 对话内容
    public List<ChatContent> Contents;
    // 选项
    public List<ChatOption> ChatOptions;

    public bool isEmpty() { return Contents == null || Contents.Count == 0; }
}

