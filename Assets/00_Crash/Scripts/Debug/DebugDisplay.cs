using UnityEngine;
using TMPro;

namespace DebugStuff
{
    public class DebugDisplay : MonoBehaviour
    {
        static string myLog = "";
        private string output;
        private string stack;

        [SerializeField] TextMeshProUGUI tmp;

        [SerializeField] Canvas canvas;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        private void Start() 
        {
            canvas.worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + "\n" + myLog;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
            }

            tmp.text = myLog;
        }
    }
}