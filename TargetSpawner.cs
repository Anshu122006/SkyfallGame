using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSpawner : MonoBehaviour
{

    public static TargetSpawner Instance { get; private set; }

    [SerializeField] private Transform[] targetArray;
    [SerializeField] private Transform centerPosition;

    private int activeSpawnPoints;
    private int pairsSpawned;
    private int iceTargetNumber;
    private int fireTargetNumber;

    private float horizontalSpawnLimit = 22;
    private float spawnDelay = 10f;
    private float targetSpeed;
    private float[] timeRange = { 1, 2 };
    private bool allTargetsSpawned;
    private bool spawnMini = false;

    private bool targetSpawned;

    private int[] subWavesLimit = new int[2];
    private int numberOfTargets;
    private int numberOfSubWaves;
    private int currentSubWave;
    private int numberOfWaves = 4;
    private int currentWave = 1;
    private PlayerStats player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = PlayerStats.Instance;
        SetNextWaveData();
    }

    private void Update()
    {
        if (player.GetGameState() == Types.GameState.gamePlaying)
        {
            spawnTargets();
        }
    }

    private void spawnTargets()
    {
        if (currentSubWave <= numberOfSubWaves)
        {
            if (!allTargetsSpawned)
            {
                pairsSpawned = 0;
                int pairIndex = 1;

                iceTargetNumber = GenerateTargetNumber();
                fireTargetNumber = (activeSpawnPoints) - iceTargetNumber;

                for (int i = 0; i < activeSpawnPoints; i++)
                {
                    if (pairIndex > 2)
                    {
                        pairIndex = 1;
                        pairsSpawned++;
                    }

                    float length = timeRange[1] - timeRange[0];
                    float baseTime = (pairIndex == 1) ? UnityEngine.Random.Range(0, length) : timeRange[1];
                    float spawnTime = pairsSpawned * timeRange[1] + baseTime;
                    FunctionTimer.Create(() =>
                    {
                        spawnRandomTarget();
                        numberOfTargets++;
                    }, spawnTime);

                    pairIndex++;
                }

                float timeDelay = pairsSpawned * timeRange[1] + spawnDelay;
                allTargetsSpawned = true;
                FunctionTimer.Create(() =>
                {
                    allTargetsSpawned = false;
                    currentSubWave++;

                }, timeDelay);
            }
        }
        else
        {
            if (numberOfTargets == 0)
            {
                currentWave++;
                if (currentWave > numberOfWaves)
                {
                    player.SetGameState(Types.GameState.gameWon);
                }
                else
                {
                    SetNextWaveData();
                }
            }
        }

    }

    private int GenerateTargetNumber()
    {
        int chance = UnityEngine.Random.Range(1, 3);
        if (chance == 1) return (int)(activeSpawnPoints / 2);
        return activeSpawnPoints - (int)(activeSpawnPoints / 2);
    }

    private void spawnRandomTarget()
    {
        Vector3 centerPoint = centerPosition.position;
        float spawnPointX = UnityEngine.Random.Range(-(int)horizontalSpawnLimit, (int)horizontalSpawnLimit + 1);
        Vector3 spawnPoint = new Vector3(spawnPointX, centerPoint.y, centerPoint.z);

        int targetIndex;
        Transform spawnedTarget = null;

        if (iceTargetNumber > 0 && fireTargetNumber > 0)
        {
            targetIndex = UnityEngine.Random.Range(0, targetArray.Length);
            spawnedTarget = Instantiate(targetArray[targetIndex]);

            if (spawnedTarget.GetComponent<Target>().GetTargetType() == Types.ElementType.fire) fireTargetNumber--;
            if (spawnedTarget.GetComponent<Target>().GetTargetType() == Types.ElementType.ice) iceTargetNumber--;
        }
        else if (fireTargetNumber == 0 && iceTargetNumber > 0)
        {
            targetIndex = UnityEngine.Random.Range(0, targetArray.Length / 2);
            spawnedTarget = Instantiate(targetArray[targetIndex]);

            iceTargetNumber--;
        }
        else if (iceTargetNumber == 0 && fireTargetNumber > 0)
        {
            targetIndex = UnityEngine.Random.Range(targetArray.Length / 2, targetArray.Length);
            spawnedTarget = Instantiate(targetArray[targetIndex]);

            fireTargetNumber--;
        }

        if (spawnedTarget != null)
        {
            spawnedTarget.position = spawnPoint;
            Vector2 moveVelocity = new Vector2(0, -1) * targetSpeed;
            spawnedTarget.GetComponent<Target>().SetMoveVelocity(moveVelocity);
            spawnedTarget.GetComponent<Target>().SetSpawnMini(spawnMini);
        }

    }

    private void SetNextWaveData()
    {
        numberOfTargets = 0;
        currentSubWave = 1;
        switch (currentWave)
        {
            case 1:
                activeSpawnPoints = 2;
                iceTargetNumber = GenerateTargetNumber();
                fireTargetNumber = (activeSpawnPoints) - iceTargetNumber;

                subWavesLimit[0] = 4;
                subWavesLimit[1] = 5;
                numberOfSubWaves = UnityEngine.Random.Range(subWavesLimit[0], subWavesLimit[1] + 1);
                currentSubWave = 1;

                targetSpeed = 3f;
                spawnMini = false;
                break;

            case 2:
                activeSpawnPoints = 4;
                iceTargetNumber = GenerateTargetNumber();
                fireTargetNumber = (activeSpawnPoints) - iceTargetNumber;

                numberOfSubWaves = UnityEngine.Random.Range(subWavesLimit[0], subWavesLimit[1] + 1);
                currentSubWave = 1;

                targetSpeed = 3.5f;
                spawnMini = false;
                break;

            case 3:
                activeSpawnPoints = 6;
                iceTargetNumber = GenerateTargetNumber();
                fireTargetNumber = (activeSpawnPoints) - iceTargetNumber;

                subWavesLimit[0]++;
                subWavesLimit[1]++;
                numberOfSubWaves = UnityEngine.Random.Range(subWavesLimit[0], subWavesLimit[1] + 1);
                currentSubWave = 1;

                targetSpeed = 4.5f;
                spawnMini = false;
                break;

            case 4:
                activeSpawnPoints = 8;
                iceTargetNumber = GenerateTargetNumber();
                fireTargetNumber = (activeSpawnPoints) - iceTargetNumber;

                subWavesLimit[0]--;
                subWavesLimit[1]--;
                numberOfSubWaves = UnityEngine.Random.Range(subWavesLimit[0], subWavesLimit[1] + 1);
                currentSubWave = 1;

                targetSpeed = 4.5f;
                spawnMini = true;
                break;
        }
        WaveManager.Instance.showWave(currentWave);
    }

    private void spawnTargets_End()
    {

        for (int i = 0; i < numberOfTargets; i++)
        {
            fireTargetNumber = UnityEngine.Random.Range(0, 2);
            iceTargetNumber = UnityEngine.Random.Range(0, 2);
            if (!targetSpawned)
            {
                float time = UnityEngine.Random.Range(timeRange[0], timeRange[1]);
                targetSpawned = true;
                FunctionTimer.Create(() =>
                {
                    spawnRandomTarget();
                    targetSpawned = false;
                }, time);
            }
        }
    }

    public void TargetDestroyed()
    {
        numberOfTargets--;
    }
}
