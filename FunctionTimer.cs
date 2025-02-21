using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer
{
    private static List<FunctionTimer> timerList;
    private static GameObject initGameObject;

    private float timer;
    private Action action;
    private bool isDestroyed;
    private GameObject gameObject;
    private string timerName;
    FunctionTimer(Action action, float timer, GameObject gameObject, string timerName)
    {
        this.timer = timer;
        this.action = action;
        isDestroyed = false;
        this.gameObject = gameObject;
        this.timerName = timerName;

    }

    private void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroySelf();
            }
        }
    }

    private static void InitIfNeeded()
    {
        if (initGameObject == null)
        {
            initGameObject = new GameObject("FunctionTimer_InitGameObject");
            timerList = new List<FunctionTimer>();
        }
    }

    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;

        private void Update()
        {
            if (onUpdate != null) onUpdate();
        }
    }

    public static FunctionTimer Create(Action action, float timer, string timerName = null)
    {
        InitIfNeeded();

        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timerName);
        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        timerList.Add(functionTimer);
        return functionTimer;
    }

    private void Removetimer(FunctionTimer functionTimer)
    {
        if (initGameObject == null) InitIfNeeded();
        timerList.Remove(functionTimer);
    }

    private void Destroytimer(string timerName)
    {
        for (int i = 0; i < timerList.Count; i++)
        {
            if (timerList[i].timerName == timerName)
            {
                timerList[i].DestroySelf();
                i--;
            }
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
        Removetimer(this);
    }
}
