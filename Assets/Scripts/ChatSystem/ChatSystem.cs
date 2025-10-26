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
            // �ڶԻ���
            && status == ChatStatus.InProcess
            // ����ѡ������
            && !chatUIScript.isSelectingOptions
            // ��˯��
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
            // ��ʼ�Ի�
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

    // ���е�ǰ�Ի�Ƭ��չʾ
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

    // �Ի�����չʾ
    private async void ShowCurrentContent()
    {
        List<ChatContent> contents = currentChatSection.Contents;
        ChatContent currentContent = contents[contentIndex];
        chatUIScript.Show().UpdateText(currentContent.Text);
        Debug.Log($"Show current chat content, index: {contentIndex}");
        // ����ֹͣ��Ӧ����
        await WaitForSeconds();
    }

    // ������չʾ��һ���Ի�����
    private async void HandleNextChatContent()
    {
        List<ChatContent> contents = currentChatSection.Contents;
        int nextContentIndex = contentIndex + 1;
        // �Ի�����δ���� ������һ������
        if (nextContentIndex < contents.Count)
        {
            contentIndex++;
            ShowCurrentContent();
        }
        else
        {
            // �Ի����ݽ���, ������������
            // 1. �Ƿ���Optionѡ�� չʾOptions
            List<ChatOption> chatOptions = currentChatSection.ChatOptions;
            if (chatOptions != null && chatOptions.Count > 0)
            {
                // ����ֹͣ��Ӧ����
                await WaitForSeconds();

                chatUIScript.ShowOptions(chatOptions);
                chatUIScript.OnConfirm += HandleOptionConfirm;
                return;
            }
            // 2. �Ƿ���Next
            ChatSection next = currentChatSection.Next;
            if (next != null)
            {
                currentChatSection = next;
                // չʾ�Ի�����
                ShowCurrentSection();
                return;
            }

            // 4. ֱ�ӽ���
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
        // ս��
        Battle,
        // �ø�
        Love,
        // ���
        Join
    }

    public int Id;
    // ѡ������
    public string Text;
    // ѡ�����
    public Action ActionWhenChatOver = Action.None;

    public List<ChatBattleConfig> ChatBattleConfig;

    public ChatLoveConfig LoveConfig;

    public ChatSection Next;
}

// �����������ݽṹ
[System.Serializable]
public class ChatSection
{
    public int Id;
    public string Name;
    // ��һ�ζԻ�
    public ChatSection Next;
    // �Ի�����
    public List<ChatContent> Contents;
    // ѡ��
    public List<ChatOption> ChatOptions;

    public bool isEmpty() { return Contents == null || Contents.Count == 0; }
}

