using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUserGUI : MonoBehaviour
{
    private uint queueSize = 15;

    private Queue logQueue = new Queue();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start up debugger logging");
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logQueue.Enqueue($"[{type}] : {logString}");
        if (type == LogType.Exception)
        {
            logQueue.Enqueue(stackTrace);
        }

        while (logQueue.Count > queueSize)
        {
            logQueue.Dequeue();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
        GUILayout.Label("\n" + string.Join("\n", logQueue.ToArray()));
        GUILayout.EndArea();
    }
}