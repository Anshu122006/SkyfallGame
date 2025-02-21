using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{

    [SerializeField] private GameObject gameLost;
    [SerializeField] private GameObject gameWon;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject unpause;
    [SerializeField] private GameObject buttons;

    [SerializeField] private GameInputManager input;

    [SerializeField] private AudioClipsSO audioClips;

    [SerializeField] private Button restart;
    [SerializeField] private Button quit;

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;

    [SerializeField] private ParticleSystem explodeEffectPrefab;
    private bool isGameEnd;
    private PlayerStats player;

    private const string HIGHSCORE = "highScore";

    private void Start()
    {
        player = PlayerStats.Instance;
        Hide(gameLost);
        Hide(gameWon);
        Hide(buttons);

        restart.onClick.AddListener(() =>
        {
            AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
            input.ResetSubscribers();
            Loader.LoadScene(Loader.SceneName.GameScene);
        });

        quit.onClick.AddListener(() =>
        {
            AudioSource.PlayClipAtPoint(audioClips.click, Vector3.zero);
            input.ResetSubscribers();
            Loader.LoadScene(Loader.SceneName.MainMenuScene);
        });
    }

    private void Update()
    {
        if (!isGameEnd)
        {
            if (player.GetGameState() == Types.GameState.gameLost)
            {
                Show(gameLost);
                Hide(pause);
                Hide(unpause);
                Show(buttons);
                score.text = player.GetScore().ToString();
                highScore.text = PlayerPrefs.GetInt(HIGHSCORE).ToString();

                ParticleSystem explodeEffect = Instantiate(explodeEffectPrefab);
                explodeEffect.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 1);
                player.gameObject.SetActive(false);

                isGameEnd = true;
            }
            if (player.GetGameState() == Types.GameState.gameWon)
            {
                Show(gameWon);
                Hide(pause);
                Hide(unpause);
                Show(buttons);

                score.text = player.GetScore().ToString();
                highScore.text = PlayerPrefs.GetInt(HIGHSCORE).ToString();

                isGameEnd = true;
            }
        }
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
