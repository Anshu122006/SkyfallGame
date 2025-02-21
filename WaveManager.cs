using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private GameObject waveObject;
    private string waveText;
    private float baseTime = 1f;
    private float increment = 0.1f;
    private float stayTime = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void showWave(int currentWave)
    {
        if (PlayerStats.Instance.GetGameState() != Types.GameState.gameLost || PlayerStats.Instance.GetGameState() != Types.GameState.gameWon)
        {
            PlayerStats.Instance.SetGameState(Types.GameState.gameWaveLoading);
            waveObject.SetActive(true);
            waveText = "WAVE-" + currentWave;
            string[] transitions = new string[waveText.Length];
            for (int i = 1; i <= waveText.Length; i++)
            {
                transitions[i - 1] = waveText.Substring(0, i);
            }
            float time = baseTime;
            for (int i = 0; i < transitions.Length; i++)
            {
                string s = transitions[i];
                FunctionTimer.Create(() =>
                {
                    wave.text = s;
                }, time);
                time += increment;
            }
            time += stayTime;
            FunctionTimer.Create(() =>
            {
                time = baseTime;
                for (int i = transitions.Length - 1; i >= 0; i--)
                {
                    string s = transitions[i];
                    FunctionTimer.Create(() =>
                    {
                        wave.text = s;
                    }, time);
                    time += increment;
                }
                FunctionTimer.Create(() =>
                {
                    waveText = "";
                    wave.text = waveText;
                    waveObject.SetActive(false);
                    PlayerStats.Instance.SetGameState(Types.GameState.gamePlaying);
                }, time);

            }, time);
        }
    }
}
