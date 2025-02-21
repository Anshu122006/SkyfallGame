using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSpawner_dummy : MonoBehaviour
{
    [SerializeField] private Transform[] targetArray;
    [SerializeField] private Transform centerPosition;

    private int numberOfTargets;

    private float spawnPointLimit = 22;
    private float[] timeRange = { 1f, 2f };
    private bool targetSpawned;

    private void Awake()
    {
        numberOfTargets = 6;
    }

    private void Update()
    {
        spawnTargets();
    }

    private void spawnTargets()
    {

        for (int i = 0; i < numberOfTargets; i++)
        {
            if (!targetSpawned)
            {
                float time = Random.Range(timeRange[0], timeRange[1]);
                targetSpawned = true;
                FunctionTimer.Create(() =>
                {
                    spawnRandomTarget();
                    targetSpawned = false;
                }, time);
            }
        }
    }

    private void spawnRandomTarget()
    {
        Vector3 centerPoint = centerPosition.position;
        float spawnPointX = Random.Range(-(int)spawnPointLimit, (int)spawnPointLimit + 1);
        Vector3 spawnPoint = new Vector3(spawnPointX, centerPoint.y, centerPoint.z);

        int targetIndex;
        Transform spawnedTarget = null;

        targetIndex = UnityEngine.Random.Range(0, targetArray.Length);
        spawnedTarget = Instantiate(targetArray[targetIndex]);

        if (spawnedTarget != null)
        {
            spawnedTarget.position = spawnPoint;
            float speed = 4f;
            spawnedTarget.GetComponent<Target_dummy>().SetMoveSpeed(speed);
        }

    }
}
