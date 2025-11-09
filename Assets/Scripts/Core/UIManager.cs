using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public UIDocument MainUIDocument;
    [SerializeField]
    public UIDocument ChatUIDocument;

    [Header("Debug")]
    public static Stack<Object> UIStack = new Stack<Object>();

    private GameManager gameManager;
    private PartyManager partyManager;

    private MainMenuUI mainMenuUI;

    // 是否在对话中
    private static bool isChating;


    public static bool IsOnTop(Object uiInstance)
    {
        if (UIStack.Count == 0) {
            return false;
        }
        return UIStack.Peek().Equals(uiInstance);
    }

    // 是否有交互UI激活
    public static bool IsUIAcitve
    {
        get {
            return UIStack.Count > 0;
        }
    }

    public static void SetChating(bool status)
    {
        isChating = status;
    }

    public static bool IsChating { 
        get { return isChating; }
    }


    private void Awake()
    {
        GameManager.OnGameInited(InitPage);
    }

    void InitPage(GameObject gameManagerObj)
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        partyManager = gameManagerObj.GetComponent<PartyManager>();
        mainMenuUI = gameManagerObj.GetComponentInChildren<MainMenuUI>();
    }

    public static void Push(Object uiInstance)
    {
        if (UIStack.Contains(uiInstance)) {
            return;
        }
        UIStack.Push(uiInstance);
    }

    public static Object Pop(Object uiInstance)
    {
        if (UIStack.Count == 0)
        {
            return null;
        }
        if (UIStack.Peek().Equals(uiInstance))
        {
            return UIStack.Pop();
        }
        return null;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
  
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C)) {
            if (UIStack.Count > 0)
            {
                while (UIStack.Count > 0) {
                    var ui = UIStack.Pop() as IUIBase;
                    ui.Close();
                }
            } else
            {
                if (IsChating)
                {
                    return;
                }
                mainMenuUI.Show();
            }
        }

        if (UIStack.Count > 0) {
            if (!GameManager.IsPaused)
            {
                GameManager.PauseGame();
            }
        } else
        {
            if (GameManager.IsPaused)
            {
                GameManager.ResumeGame();
            }
        }
    }
}
