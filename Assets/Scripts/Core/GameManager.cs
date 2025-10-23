using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

// 定义带返回值的回调类型
public delegate void OnGameInitedCallback(GameObject obj);

// Global Manager
public class GameManager : MonoBehaviour
{
    [SerializeField] public UIDocument ChatUIDocument;

    const string GAME_MANAGER_KEY = "GameManager";

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
}
