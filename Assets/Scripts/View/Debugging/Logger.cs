using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class Logger : MonoBehaviour
    {
        [Serializable]
        private class Severity
        {
            public LogType type;
            public string color = "white";
            [Range(1f, 200f)] public float size = 50f;
            public string prefix = "";
        }
        [SerializeField] private KeyCode toggleLogKey = KeyCode.Tab;
        [SerializeField] private List<Severity> severities = new();
        [SerializeField, Range(0f, 100f)] private float paddingLeft = 10f;
        [SerializeField, Range(0f, 100f)] private float paddingRight = 10f;
        [SerializeField, Range(0f, 100f)] private float paddingUp = 10f;
        [SerializeField, Range(0f, 100f)] private float paddingDown = 10f;
        private string log = "";
        private bool show;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        private void Update()
        {
            if (Input.GetKeyDown(toggleLogKey))
            {
                show = !show;
            }
        }
        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }
        private void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }
        private void OnGUI()
        {
            if (!show) return;
            float width = Screen.width - paddingLeft - paddingRight;
            float height = Screen.height - paddingUp - paddingDown;
            Rect box = new Rect(paddingLeft, paddingUp, width, height);
            GUI.Box(box, "");  // Use box to get background
            GUI.Label(box, log);  // Use label to avoid centered text
        }
        private void Log(string logString, string stackTrace, LogType type)
        {
            Severity severity = severities.Find(s => s.type == type);
            if (severity == null)
            {
                logString = $"(Severity of LogType {type} not registered in Logger) {logString}";
            } else
            {
                float size = severity.size;
                string color = severity.color;
                string prefix = severity.prefix;
                logString = $"<size={size}><color={color}><b>{prefix}</b></color>{logString}</size>";
            }
            log = $"{logString}\n{log}";
            if (log.Length > 5000)
                log = log[..4000];
        }
    }
}