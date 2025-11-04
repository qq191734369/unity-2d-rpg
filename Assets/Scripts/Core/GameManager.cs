using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// 定义带返回值的回调类型
public delegate void OnGameInitedCallback(GameObject obj);

// Global Manager
public class GameManager : MonoBehaviour
{
    [SerializeField] public UIDocument ChatUIDocument;

    const string GAME_MANAGER_KEY = "GameManager";
    const string PLAYER_TAG = "Player";

    public static System.Action Ready;

    public static GameObject Instance;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeGameManager()
    {
        Addressables.InstantiateAsync(GAME_MANAGER_KEY).Completed += OnInitialized;
    }

    static void OnInitialized(AsyncOperationHandle<GameObject> op)
    {
        GameManager.Instance = op.Result;
        DontDestroyOnLoad(op.Result);
        Ready?.Invoke();
    }

    public static void OnGameInited(OnGameInitedCallback callback)
    {
        if (GameManager.Instance != null) {
            callback(GameManager.Instance);
        } else
        {
            GameManager.Ready += () => callback(GameManager.Instance);
        }
    }

    public static void SavePosiiton()
    {
        PartyManager partyManager = Instance.GetComponent<PartyManager>();
        DataManager dataManager = Instance.GetComponent<DataManager>();
        GameObject player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        PartyMemberRender partyMemberRender = player.GetComponent<PartyMemberRender>();
        Scene scene = SceneManager.GetActiveScene();

        CharacterEntity playerInfo = dataManager.gameGlobalData.PlayerInfo;
        playerInfo.Scene = scene.name;
        playerInfo.Position = player.transform.position;

        partyMemberRender.MemberVisualList.ForEach(d =>
        {
            CharacterEntity characterInfo = d.GetComponent<CharactorInfo>().CharacterInfo;
            characterInfo.Scene = scene.name;
            characterInfo.Position = d.transform.position;
        });
    }
}
