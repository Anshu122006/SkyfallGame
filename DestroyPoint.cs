using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    [SerializeField] private Transform[] destroyPoints;
    public static DestroyPoint Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetDestroyPoint(int sideIndex)
    {
        return destroyPoints[sideIndex];
    }
}
