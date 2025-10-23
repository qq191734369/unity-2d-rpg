using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadSceneMask : MonoBehaviour
{
    const string UssFade = "fade";

    VisualElement mask;

    WaitUntil waitUntilSceneLoaded;

    public static event System.Action ShowLoadingScreen;

    private void Awake()
    {
        mask = GetComponent<UIDocument>().rootVisualElement.Q("Mask");
        waitUntilSceneLoaded = new WaitUntil(() => SceneLoader.IsSceneLoaded);

        SceneLoader.LoadingStarted += FadeOut;
        SceneLoader.LoadingCompleted += FadeIn;
    }

    IEnumerator ActivateLoadedSceneCorotine()
    {
        yield return waitUntilSceneLoaded;
        SceneLoader.ActivateLoadedScene();
    }

    void FadeOut()
    {
        mask.AddToClassList(UssFade);
        mask.RegisterCallback<TransitionEndEvent>(OnFadeOutEnd);
    }

    void FadeIn()
    {
        mask.RemoveFromClassList(UssFade);
    }

    void OnFadeOutEnd(TransitionEndEvent transitionEndEvent)
    {
        mask.UnregisterCallback<TransitionEndEvent>(OnFadeOutEnd);
        if (SceneLoader.ShowLoading)
        {
            ShowLoadingScreen?.Invoke();
        } else
        {
            StartCoroutine(ActivateLoadedSceneCorotine());
        }
    }
}
