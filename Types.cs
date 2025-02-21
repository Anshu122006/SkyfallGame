using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types
{
    public enum Side
    {
        LeftLeft = 0,
        Left = 1,
        Center = 2,
        Right = 3,
        RigitRight = 4,
    }

    public enum ElementType
    {
        fire,
        ice,
    }

    public enum TargetType
    {
        normal,
        mini,
    }

    public enum GameState
    {
        gameStart,
        gamePlaying,
        gameWaveLoading,
        gameLost,
        gameWon,
    }

    public enum ControlType
    {
        move,
        rotate,
    }
}
