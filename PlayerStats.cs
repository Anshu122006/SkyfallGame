using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats Instance { get; private set; }

    private int waveNumber;
    private int playerHP = 100;
    private int playerMHP = 100;
    private int playerScore = 0;
    private float startTime = 0.5f;
    private Types.GameState gameState;

    private void Awake()
    {
        Instance = this;
        gameState = Types.GameState.gameStart;
    }

    private void Start()
    {
        FunctionTimer.Create(() =>
        {
            gameState = Types.GameState.gamePlaying;
        }, startTime);
    }

    public void SetHP(int playerHP)
    {
        this.playerHP = playerHP;
    }

    public int GetHP()
    {
        return playerHP;
    }

    public float GetMHP()
    {
        return playerMHP;
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void SetScore(int playerScore)
    {
        this.playerScore = playerScore;
    }

    public Types.GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(Types.GameState gameState)
    {
        this.gameState = gameState;
    }
}
