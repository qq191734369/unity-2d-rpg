using UnityEngine;

public class StoreSystem : MonoBehaviour
{
    [SerializeField]
    string StoreKey;

    private UIManager uiManager;
    private StoreUI storeUI;

    private void Awake()
    {
        GameManager.OnGameInited(Init);
    }

    private void Init(GameObject gameManagerObj) {
        uiManager = gameManagerObj.GetComponent<UIManager>();
        storeUI = uiManager.StoreUIDocument.GetComponent<StoreUI>();
    }

    public void ShowStoreUI()
    {
        storeUI.Show(StoreKey);
    }
}
