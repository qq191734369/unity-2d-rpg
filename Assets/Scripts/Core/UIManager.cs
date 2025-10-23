using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;

public class UIManager : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button exitGameBtn;
    private GameObject ugameObject;

    private void Awake()
    {
        CreateGameMenuDocument();
    }

    void CreateGameMenuDocument()
    {
        // 创建GameObject并添加UIDocument组件
        ugameObject = new GameObject();
        ugameObject.name = "Game Menu";
        ugameObject.transform.parent = this.transform;
        uiDocument = ugameObject.AddComponent<UIDocument>();

        // 设置UI Document的配置
        uiDocument.visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/MainMenu/MainMenu");
        uiDocument.panelSettings = Resources.Load<PanelSettings>("Settings/UI Toolkit/PanelSettings");
        uiDocument.sortingOrder = 1000;

        exitGameBtn = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        exitGameBtn.clicked += ExitGame;

        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
  
    }

    void ExitGame()
    {
        Debug.Log("clicked exit game btn");
        GameActions.ExitGame();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            uiDocument.rootVisualElement.style.display = uiDocument.rootVisualElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            Debug.Log("click esc");
        }
    }
}
