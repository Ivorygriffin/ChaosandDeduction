using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;
    CustomNetworkManager networkManager;

    [System.Serializable]
    public struct tip
    {
        public string text;
        public Sprite icon;
    }
    public tip[] tips;

    public Canvas[] canvas;
    public Canvas loadingScreen;
    public TMP_Text tipText;
    public Image tipImage;
    public Slider progressSlider;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        networkManager = (CustomNetworkManager)NetworkManager.singleton;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void LoadMainScene()
    {
        networkManager.ChangeScene();
    }

    public void LoadScreen() //TODO: determine how to call this function after the loadingSceneAsync has been declared (both on server and client)
    {
        foreach (Canvas c in canvas)
            c.enabled = false;

        loadingScreen.enabled = true;

        int tipIndex = Random.Range(0, tips.Length);
        tipText.text = tips[tipIndex].text;
        tipImage.sprite = tips[tipIndex].icon;

        StartCoroutine(LoadAsyncScene());
    }

    AsyncOperation asyncLoad;
    IEnumerator LoadAsyncScene()
    {
        yield return new WaitUntil(() => NetworkManager.loadingSceneAsync != null);

        asyncLoad = NetworkManager.loadingSceneAsync;
        asyncLoad.allowSceneActivation = false;

        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            progressSlider.value = asyncLoad.progress;

            //scene has loaded as much as possible,
            // the last 10% can't be multi-threaded
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            else
                asyncLoad.allowSceneActivation = false;

            yield return null;
        }
    }
}
