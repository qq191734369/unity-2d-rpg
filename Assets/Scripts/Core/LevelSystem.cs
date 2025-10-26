using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private DataManager dataManager;

    private void Awake()
    {
        GameManager.OnGameInited(init);
    }

    private void init(GameObject gameManagerObj)
    {
        dataManager = gameManagerObj.GetComponent<DataManager>();
    }
}
