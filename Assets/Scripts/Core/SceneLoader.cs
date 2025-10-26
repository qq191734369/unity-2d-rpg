using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;

    static SceneInstance loadedInstance;

    public const string MAIN_MENU_SCENE_KEY = "MainMenu";
    public const string WORLD_MAP = "WorldMap";
    public const string DEBUG_SCENE = "DebugScene";
    public const string BATTLE_SCENE = "BattleScene";

    public static event System.Action LoadingStarted;
    public static event System.Action<float> IsLoading;
    public static event System.Action LoadingSucceeded;
    public static event System.Action LoadingCompleted;

    public static Dictionary<string, SceneParams> SceneParams = new Dictionary<string, SceneParams>();

    public static bool ShowLoading { get; private set; }
    public static bool IsSceneLoaded { get; private set; }

    void Awake()
    {
        instance = this;
    }


    static IEnumerator LoadAddressableSceneCoroutine(
        object key,
        bool showLoading,
        bool loadSceneAdditively = false,
        bool activateOnLoad = false
    )
    {
        LoadSceneMode loadSceneMode =
            loadSceneAdditively
            ? LoadSceneMode.Additive
            : LoadSceneMode.Single;
        Debug.Log("Load Scene Start");
        var asyncLoadHandler = Addressables.LoadSceneAsync(key, loadSceneMode);
        LoadingStarted?.Invoke();
        ShowLoading = showLoading;
        while (asyncLoadHandler.Status != AsyncOperationStatus.Succeeded)
        {
            IsLoading?.Invoke(asyncLoadHandler.PercentComplete);
            yield return null;
        }

        if (activateOnLoad)
        {
            LoadingCompleted?.Invoke();
            Debug.Log("LoadingCompleted");
            yield break;
        }

        LoadingSucceeded?.Invoke();
        IsSceneLoaded = true;
        Debug.Log("LoadingSucceeded");

        loadedInstance = asyncLoadHandler.Result;
    }

    public static void ActivateLoadedScene()
    {
        loadedInstance.ActivateAsync().completed += op =>
        {
            IsSceneLoaded = false;
            loadedInstance = default;
            LoadingCompleted?.Invoke();
            Debug.Log("LoadingCompleted");
        };
    }

    static public void LoadAddressableScene(
        object key,
        bool showLoading = false,
        bool loadSceneAdditively = false,
        bool activateOnLoad = false
    )
    {
        instance.StartCoroutine(LoadAddressableSceneCoroutine(key, showLoading, loadSceneAdditively, activateOnLoad));
    }

    static public void LoadBattleScene(SceneParams p)
    {
        SceneParams.Add(BATTLE_SCENE, p);
        SceneLoader.LoadAddressableScene(
            BATTLE_SCENE
        );
    }
}

public class SceneEnemyInfo
{
    public string Name;
    public int Level;
}

public class SceneParams
{
    public List<SceneEnemyInfo> EnemyList;
}
