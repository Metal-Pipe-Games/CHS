using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransporter : MonoBehaviour
{
    public Image loadingBar;
    public Image preview;

    static string TargetSceneName = "";
    static int TargetSceneIndex = 2;
    static Sprite TargetPreview;
    AsyncOperation loadingScene;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(TargetSceneIndex);
        if(TargetSceneIndex != -1)loadingScene = SceneManager.LoadSceneAsync(TargetSceneIndex);
        else if(TargetSceneName != "") loadingScene = SceneManager.LoadSceneAsync(TargetSceneName);
        if (TargetPreview) preview.sprite = TargetPreview;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingScene != null && !loadingScene.isDone)
        {
            float progress = Mathf.Clamp01(loadingScene.progress / 0.9f);
            loadingBar.fillAmount = progress;
        }
    }

    public static void GoToScene(string targetScene)
    {
        TargetSceneName = targetScene;
        TargetSceneIndex = -1;
        LoadSelf();
    }
    public static void GoToScene(int targetScene)
    {
        TargetSceneIndex = targetScene;
        LoadSelf();
    }
    public static IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    public static void GoToScene(string targetScene, Sprite targetPreview)
    {
        TargetSceneName = targetScene;
        TargetSceneIndex = -1;
        TargetPreview = targetPreview;
        LoadSelf();
    }
    public static void GoToScene(int targetScene, Sprite targetPreview)
    {
        TargetSceneIndex = targetScene;
        TargetPreview = targetPreview;
        LoadSelf();
    }

    static AsyncOperation ownLoader;
    public static void LoadSelf()
    {
        ownLoader = SceneManager.LoadSceneAsync(1);
    }
}
