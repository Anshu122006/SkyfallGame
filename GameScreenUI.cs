using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreenUI : MonoBehaviour
{

    public event EventHandler OnSwitchControl;

    public static GameScreenUI Instance { get; private set; }

    [SerializeField] private GameInputManager input;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resume;
    [SerializeField] private Button restart;
    [SerializeField] private Button quit;
    [SerializeField] private Button onScreenControls;
    [SerializeField] private Button switchControls;
    [SerializeField] private Button toggleSound;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject unpauseScreen;
    [SerializeField] private GameObject controlScreen;
    [SerializeField] private GameObject controlTick;
    [SerializeField] private GameObject[] soundState;
    [SerializeField] private GameObject[] controlType;

    [SerializeField] private AudioClipsSO audioClips;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Hide(pauseScreen);
        input.OnPause += Input_OnPause;
        onScreenControls.onClick.AddListener(ToggleScreenControls);
        switchControls.onClick.AddListener(SwitchControls);
        restart.onClick.AddListener(RestartScene);
        quit.onClick.AddListener(QuitScene);
        toggleSound.onClick.AddListener(TogggleSound);
    }

    private void Input_OnPause(System.Object sender, EventArgs e)
    {
        if (PlayerStats.Instance.GetGameState() != Types.GameState.gameLost && PlayerStats.Instance.GetGameState() != Types.GameState.gameWon)
        {
            if (!pauseScreen.activeSelf)
            {
                Show(pauseScreen);
                Hide(unpauseScreen);
                Time.timeScale = 0f;
                AudioSource.PlayClipAtPoint(audioClips.pause, Vector3.zero);
            }
            else
            {
                Hide(pauseScreen);
                Show(unpauseScreen);
                Time.timeScale = 1f;
                AudioSource.PlayClipAtPoint(audioClips.unpause, Vector3.zero);
            }
        }
    }

    private void ToggleScreenControls()
    {
        if (controlType[0].activeSelf)
        {
            if (controlScreen.activeSelf)
            {
                Hide(controlScreen);
                Hide(controlTick);
            }
            else
            {
                Show(controlScreen);
                Show(controlTick);
            }
        }
        AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
    }

    private void TogggleSound()
    {
        if (soundState[0].activeSelf)
        {
            Hide(soundState[0]);
            Show(soundState[1]);
            AudioListener.volume = 0;
        }
        else
        {
            Hide(soundState[1]);
            Show(soundState[0]);
            AudioListener.volume = 1;
        }
        AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
    }

    private void SwitchControls()
    {
        if (controlType[0].activeSelf)
        {
            Hide(controlType[0]);
            Show(controlType[1]);
            if (controlTick.activeSelf) Hide(controlScreen);
        }
        else
        {
            Hide(controlType[1]);
            Show(controlType[0]);
            if (controlTick.activeSelf) Show(controlScreen);
        }
        OnSwitchControl?.Invoke(this, EventArgs.Empty);
        AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
    }

    private void RestartScene()
    {
        AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
        input.ResetSubscribers();
        Loader.LoadScene(Loader.SceneName.GameScene);
    }

    private void QuitScene()
    {
        AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
        input.ResetSubscribers();
        Loader.LoadScene(Loader.SceneName.MainMenuScene);
    }

    private void Show(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    private void Hide(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
