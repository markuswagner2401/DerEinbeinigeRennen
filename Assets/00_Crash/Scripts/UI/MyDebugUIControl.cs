using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ObliqueSenastions.UISpace
{

    public class MyDebugUIControl : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI fps;
        [SerializeField] TextMeshProUGUI debugLog;

        [SerializeField] bool showAtStart = true;
        [SerializeField] bool showFrameRate = true;
        [SerializeField] Color textColor;
        [SerializeField] int textSize;



        float deltaTime = 0.0f;

        void Start()
        {
            ShowUI(showAtStart);
            fps.color = textColor;
            fps.fontSize = textSize;


            debugLog.color = textColor;
            debugLog.fontSize = textSize;


            debugLog.color = textColor;
            debugLog.fontSize = textSize;

        }


        void Update()
        {
            if (!showFrameRate) return;

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            float msec = deltaTime * 1000.0f;

            this.fps.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);



        }

        public void ShowUI(bool value)
        {
            fps.transform.parent.gameObject.SetActive(value);
            debugLog.transform.parent.gameObject.SetActive(value);

        }

        public void MyDebugLog(string text)
        {
            print("my debug log");
            string capturedText = debugLog.text;
            debugLog.text = text + "\n" + capturedText;
        }
    }
}
