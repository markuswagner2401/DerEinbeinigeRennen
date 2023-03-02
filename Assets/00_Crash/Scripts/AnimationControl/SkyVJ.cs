using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyVJ : MonoBehaviour
{
    [SerializeField] Renderer rend = null;

    [Tooltip("to share property block")]
    [SerializeField] MaterialPropertiesFader_2 fader = null; //to share property block
    [SerializeField] SkyStruct[] skies;

    [SerializeField] Texture debugTex;
    [SerializeField] string refDebugTex;
    

    [System.Serializable]
    struct SkyStruct
    {
        public string note;
        public Cubemap cubemap;
        public float fadeInTime;

        public float waitTime;
        public AnimationCurve curve;
        public bool automaticlyFadeNext;
    }

    [SerializeField] string blendValueRef;
    [SerializeField] string TexARef;
    [SerializeField] string TexBRef;

    [SerializeField] int startSkiesIndex = 0;

    Cubemap[] cubemapsAB = new Cubemap[2];
    enum SkyTable
    {
        A,
        B
    }
    
    SkyTable freeSkyTable = SkyTable.B;





    //public Renderer rend = new Renderer();

    int currentIndex;
    int lastIndex;




    private MaterialPropertyBlock block;

    void OnEnable()
    {
        
        


        // rend = GetComponent<Renderer>();

        // InitialSetup();
    }

    IEnumerator GetPropertyBlock(){
        if(fader == null) yield break;
        while(block == null){
            block = fader.GetPropertyBlock();
            yield return null;
        }

        InitializeBlock();
        
        yield break;
    }

    // private void InitialSetup()
    // {
        
    // }

    void Start()
    {
        if(fader != null){
            StartCoroutine(GetPropertyBlock()); 
        }

        else{
            block = new MaterialPropertyBlock();
            InitializeBlock();
        }

        

        
        
        
    }

    void InitializeBlock(){
        if (block != null){
            block.SetTexture(TexARef, skies[startSkiesIndex].cubemap);

            block.SetTexture(refDebugTex, debugTex);

            rend.SetPropertyBlock(block);
        }
    }

    
    // void Update()
    // {
        
    //     // at the end
    //     // rend.SetPropertyBlock(block);
    // }

    public void FadeToNextSky()
    {
        print("fade To nextSky");
        StartCoroutine(WaitAndFadeToNext(skies[currentIndex].waitTime));
        
    }

    IEnumerator WaitAndFadeToNext(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetNextIndex();
        FadeToSky(currentIndex);
        yield break;
    }

    private void SetNextIndex()
    {
        currentIndex += 1;
        if(currentIndex >= skies.Length)
        {
            currentIndex = 0;
        }
    }

    public void FadeToSky(int index)
    {
        lastIndex = currentIndex;
        currentIndex = index;
        SetFreeSkyTable(currentIndex);
        StartCoroutine(FadeSkiesAB(currentIndex));

    }

    private void SetFreeSkyTable(int index)
    {
        if(freeSkyTable == SkyTable.A)
        {
            cubemapsAB[0] = skies[index].cubemap;
            block.SetTexture(TexARef, cubemapsAB[0]);
            freeSkyTable = SkyTable.B;
            return;
        }

        if(freeSkyTable == SkyTable.B)
        {
            cubemapsAB[1] = skies[index].cubemap;
            block.SetTexture(TexBRef, cubemapsAB[1]);
            freeSkyTable = SkyTable.A;
            return;
        }
    }

    IEnumerator FadeSkiesAB(int index)
    {
        float timer = 0;

        while (timer <= skies[index].fadeInTime)
        {
             timer += Time.deltaTime;
             float newValue = skies[index].curve.Evaluate(timer / skies[index].fadeInTime);
             if(freeSkyTable == SkyTable.B) // => fading to A
             {
                 newValue = 1-newValue;
             }
             block.SetFloat(blendValueRef, newValue);
             rend.SetPropertyBlock(block);
             yield return null;
        }
        if (skies[index].automaticlyFadeNext)
        {
            FadeToNextSky();
        }
        yield break;
    }
}
