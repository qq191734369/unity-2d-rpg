using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    [SerializeField] AssetReference woldMapScene;
    [SerializeField] bool debugMode = true;

    Button startBtn;
    Button exitBtn;

    private void Awake()
    {
        VisualElement rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        startBtn = rootVisualElement.Q<Button>("StartBtn");
        exitBtn = rootVisualElement.Q<Button>("ExitBtn");
    }

    private void OnEnable()
    {
        startBtn.clicked += StartGame;
        exitBtn.clicked += ExitGame;
    }

    private void OnDisable()
    {
        startBtn.clicked -= StartGame;
        exitBtn.clicked -= ExitGame;
    }

    void StartGame()
    {
        // disable all player's inputs
        // game settings init
        // load default scene
        if (debugMode)
        {
            SceneLoader.LoadAddressableScene(SceneLoader.DEBUG_SCENE);
        }
        else {
            SceneLoader.LoadAddressableScene(woldMapScene);
        }
    }

    void ExitGame()
    {
        GameActions.ExitGame();
    }
}
