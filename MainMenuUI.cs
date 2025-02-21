using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button exitGame;
    [SerializeField] private AudioClipsSO audioClips;

    private void Start()
    {
        play.onClick.AddListener(() =>
        {
            AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
            Loader.LoadScene(Loader.SceneName.GameScene);
        });
        exitGame.onClick.AddListener(() =>
        {
            AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
            Application.Quit();
        });
    }
}
