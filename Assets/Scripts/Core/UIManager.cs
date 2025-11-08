using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public UIDocument uiDocument;

    [Header("Debug")]
    public static Stack<Object> UIStack = new Stack<Object>();

    private GameManager gameManager;
    private PartyManager partyManager;

    private MainMenuUI mainMenuUI;

    public static bool IsOnTop(Object uiInstance)
    {
        if (UIStack.Count == 0) {
            return false;
        }
        return UIStack.Peek().Equals(uiInstance);
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
        UIStack.Push(uiInstance);
    }

    public static Object Pop(Object uiInstance)
    {
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
                UIStack.Push(mainMenuUI);
                mainMenuUI.Show();
            }
        }
    }
}
