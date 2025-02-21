using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum SceneName
    {
        GameScene,
        LoadingScene,
        MainMenuScene,
    }
    private static SceneName sceneName;

    public static void LoadScene(SceneName sceneName)
    {
        Loader.sceneName = sceneName;
        SceneManager.LoadScene(SceneName.LoadingScene.ToString());

    }
    public static void LoadSceneCallback()
    {
        SceneManager.LoadScene(sceneName.ToString());
        Time.timeScale = 1f;
    }
}
