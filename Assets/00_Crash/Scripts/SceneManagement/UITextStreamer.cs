using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextStreamer : MonoBehaviour
{
    //[SerializeField] Text text;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameObject textStreamParent;
    [SerializeField] string textStreamTag;

    [SerializeField] string textStreamParentTag;

    [SerializeField] UIText[] uiTexts;

    [SerializeField] bool turnOffAtStart = true;

    int currentIndex = 0;

    [System.Serializable]
    struct UIText
    {
        public string text;

        public string note;
        public Color textColor;
        public int textSize;
        public FontStyles fontStyle;
        
        public bool playNextAutomatic;
        public float waitTime;
        public bool turnOutAutomatic;
        public float turnOutTime;
    }


    void Start()
    {
        if (text == null)
        {
            text = GameObject.FindGameObjectWithTag(textStreamTag).GetComponent<TextMeshProUGUI>();
        }

        if(textStreamParent == null)
        {
            textStreamParent = GameObject.FindGameObjectWithTag(textStreamParentTag);
        }
        
        if(turnOffAtStart)
        {
            ShowUI(false);
        }
    }


    void Update()
    {
        
    }

    public void ShowText(string currentText, Color color, FontStyles style, int size){
        ShowUI(currentText != "");

       
        text.text = currentText;
        text.color = color;
        text.fontStyle = style;
        text.fontSize = size;
        
    }

    public void ShowText(string currentText){
        ShowUI(currentText != "");
        text.text = currentText;
    }

    public void ShowUI(bool value)
    {
        textStreamParent.SetActive(value);
    }




    ////// old method

    public void ReinitializeTextUI()
    {
        text = GameObject.FindGameObjectWithTag(textStreamTag).GetComponent<TextMeshProUGUI>();
        ShowUI(false);
    }

    public void SetNewText()
    {
        ShowUI(true);
        if (currentIndex > uiTexts.Length - 1) return;
        

        text.text = uiTexts[currentIndex].text;
        text.color = uiTexts[currentIndex].textColor;
        text.fontSize = uiTexts[currentIndex].textSize;
        text.fontStyle = uiTexts[currentIndex].fontStyle;
        
        SetNextIndex();

        PlayNext();
        TrunOut();
    }

    private void TrunOut()
    {
        if (uiTexts[currentIndex - 1].turnOutAutomatic)
        {
            print("turn out");
            StartCoroutine(TurnOffAutomatic(uiTexts[currentIndex - 1].turnOutTime));
        }
    }

    private void PlayNext()
    {
        if (uiTexts[currentIndex - 1].playNextAutomatic)
        {
            StartCoroutine(PlayNextAfterWait(uiTexts[currentIndex - 1].waitTime));
        }
    }

    IEnumerator PlayNextAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetNewText();
        yield break;
    }

    IEnumerator TurnOffAutomatic(float waitTime)
    {
        print("turn out automatic");
        yield return new WaitForSeconds(waitTime);
        
        ShowUI(false);
        yield break;
    }

    private void SetNextIndex()
    {
        
        currentIndex += 1;
    }

    


}
