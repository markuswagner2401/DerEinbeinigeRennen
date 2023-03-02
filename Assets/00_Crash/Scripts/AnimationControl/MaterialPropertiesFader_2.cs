using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MaterialPropertiesFader_2 : MonoBehaviour
{
    [SerializeField] Renderer[] rend = null;
    [SerializeField] SkinnedMeshRenderer[] skm = null;

    //[SerializeField] Material materialAsset;
    [SerializeField] bool skinnedMesh = true;

    //[SerializeField] bool changeMaterialAsset = false;

    [SerializeField] bool usePropertyBlock = true;

    [SerializeField] UnityEvent doOnStart;
    private MaterialPropertyBlock block = null;

    [SerializeField] FloatChanger[] floatChangers;

    [System.Serializable]
    struct FloatChanger
    {
        public string note;
        public string propRef;
        public float targetValue;

        public float randomRangeMin;
        public float randomRangeMax;
        public float duration;

        public float pushFactor;

        public bool playFloat;

        public float playSpeed;

        
        public AnimationCurve curve;
        public bool interrupted;

        public bool automaticlyPlayNext;

        public float waitTime;

        public bool playDaumenkino;
        
    }

    [SerializeField] TextureChanger[] textureChangers;

    [System.Serializable]

    struct TextureChanger
    {
        public string note;
        public Texture texture;
        public string texPropRef;

        public float fadeInDuration;
        public float fadeDurationMin;

        public float fadeDurationMax;

        public AnimationCurve curve;

    }

    [SerializeField] TextureFader textureFader;

    [System.Serializable]
    public struct TextureFader
    {
        public string note;
        public string texRefA;
        public string texRefB;
        public string faderRef;
        public bool faderIsLeft;

        public bool isInterrupted;

        
    }

    [SerializeField] MaterialChanger[] materialChangers;

    [System.Serializable]

    struct MaterialChanger
    {
        public string note;
        public Material material;

        public int materialIndex;
    }

    [SerializeField] float daumenkinoTimingMin = 1f;
    [SerializeField] float daumenkinoChangeMax = 2f;

    int currentMaterialIndex = 0;



    [SerializeField] bool setFloatsToCurrent;
    [SerializeField] bool stetTexturesToCurrent = false;
    

    int currentTextureIndex = 0;

   


    void OnEnable()
    {
        
        block = new MaterialPropertyBlock();

        if(setFloatsToCurrent)
        {
            //if(changeMaterialAsset) return;

            foreach (var parameter in floatChangers)
                {
                    if(skinnedMesh)
                        {
                            block.SetFloat(parameter.propRef, skm[0].sharedMaterial.GetFloat(parameter.propRef));
                            print("set float to: " + skm[0].sharedMaterial.GetFloat(parameter.propRef));
                        }

                    else
                    {
                        block.SetFloat(parameter.propRef, rend[0].sharedMaterial.GetFloat(parameter.propRef));
//                        print("set float to: " + rend.material.GetFloat(parameter.propRef));
                    }

                    
                }
        }



        for (int i = 0; i < floatChangers.Length; i++)
        {
            if(floatChangers[i].playFloat)
            {
                PlayFloat(i);
            }
        }

        if(stetTexturesToCurrent)
        {
            foreach (var textureChanger in textureChangers)
            {
                block.SetTexture(textureChanger.texPropRef, textureChanger.texture);
            }
        }

        if(textureFader.faderRef != ""){
            textureFader.faderIsLeft = block.GetFloat(textureFader.faderRef) == 0 ? true : false;
        }
        

        


        // rend = GetComponent<Renderer>();

        // InitialSetup();
    }

    

    private void Start() 
    {
        doOnStart.Invoke();
    }

    void Update()
    {
        if(usePropertyBlock)
        {
            if(skinnedMesh)
            {
                foreach (var item in skm)
                {
                    if(item.enabled){
                        item.SetPropertyBlock(block); 
                    }
                    
                   
                }
                
            }

            else
            {
                foreach (var item in rend)
                {
                    if(item.enabled){
                        item.SetPropertyBlock(block); 
                    }
                   
                }
                
                
            }

        }

        // if(changeMaterialAsset){
        //     foreach (var item in floatChangers)
        //     {
        //         rend[0].material.SetFloat(item.propRef, block.GetFloat(item.propRef));
        //     }
        // }

        
        
    }

    public SkinnedMeshRenderer GetSkinnedMeshRenderer(int index){
        return skm[index];
    }

    public void PlayFloat(int index)
    {
        print("play float");
        StartCoroutine(PlayFloatRoutine(index));
    }

    public void PlayFloat(string name)
    {
        int index = GetFloatChangerIndexOfName(name);
        if(index >= 0)
        {
            StartCoroutine(PlayFloatRoutine(GetFloatChangerIndexOfName(name)));
        }

        else
        {
            print("no float changer with this name found at" + gameObject.name);
        }
        
        
    }

    private int GetFloatChangerIndexOfName(string name)
    {
        for (int i = 0; i < floatChangers.Length; i++)
        {
            if(floatChangers[i].note == name)
            {
                print("float changer found: " + name);
                return i;
                
            }
            else
            {
                print("not matching: " + name + " " + floatChangers[i].note );
                continue;
            }
        }

        return -1;
    }

    IEnumerator PlayFloatRoutine(int index)
    {
        float currentValue = block.GetFloat(floatChangers[index].propRef);
        while (floatChangers[index].playFloat)
        {
            currentValue += Time.unscaledDeltaTime * floatChangers[index].playSpeed;
            block.SetFloat(floatChangers[index].propRef, currentValue);

            if(skinnedMesh)
             {
                foreach (var item in skm)
                {
                  item.SetPropertyBlock(block);  
                }
                
                
             }

             else
             {
                foreach (var item in rend)
                {
                  item.SetPropertyBlock(block);  
                }
                
                 
             }

            yield return null;
        }
        yield break;
    }
 
    

    
    

    public MaterialPropertyBlock GetPropertyBlock(){
        return block;
    }

    public void ChangeFloat(string name)
    {
        int index = GetFloatChangerIndexOfName(name);
        if(index >= 0)
        {
            ChangeFloat(index);
        }

        else
        {
            print("no float changer with this name found at" + gameObject.name);
                
        }
        
    }

    public void ChangeFloat(int index)
    {
        StartCoroutine(InterruptAndChangeFloatR(index));
    }

    

    IEnumerator InterruptAndChangeFloatR(int index)
    {
        floatChangers[index].interrupted = true;
        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(ChangeFloatR(index));
        yield break;
    }

    IEnumerator ChangeFloatR(int index)
    {
        // Material material;


        // if(!usePropertyBlock)
        // {
        //     if(changeMaterialAsset && materialAsset != null)
        //     {
        //         material = materialAsset;
        //     }

        //     else
        //     {
        //         if(skinnedMesh)
        //         {
        //             material = skm[0].material;   
                    
                    
        //         }
        //         else
        //         {   
        //             material = rend[0].material;
        //         }
        //     }
            
        // }

        // else
        // {
        //     material = null;
        // }
        
        
        floatChangers[index].interrupted = false;
        float timer = 0f;

 //       print("change float " + floatChangers[index].note);

        float startValue = block.GetFloat(floatChangers[index].propRef);

        while (timer <= floatChangers[index].duration && !floatChangers[index].interrupted)
        {
            
             timer += Time.unscaledDeltaTime;
             float newValue = Mathf.Lerp(startValue, floatChangers[index].targetValue, floatChangers[index].curve.Evaluate(timer / floatChangers[index].duration));
             block.SetFloat(floatChangers[index].propRef, newValue);
            
//                print("change float " + floatChangers[index].note + " " + block.GetFloat(floatChangers[index].propRef));
             // print("Set Float: " + floatChangers[index].note  +  block.GetFloat(floatChangers[index].propRef));
             

             if(!usePropertyBlock)
             {
                
                //materialAsset.SetFloat(floatChangers[index].propRef, newValue);
                //print("Set Float: " + "Material: " + material.name + " " + floatChangers[index].propRef + " Value: " + block.GetFloat(floatChangers[index].propRef));
                
                if(skinnedMesh)
                {
                    
                    skm[0].sharedMaterial.SetFloat(floatChangers[index].propRef, block.GetFloat(floatChangers[index].propRef));
                }

                else
                {
                    rend[0].sharedMaterial.SetFloat(floatChangers[index].propRef, block.GetFloat(floatChangers[index].propRef));
                    //print(rend[0].sharedMaterial.name + " " + "Set Float " + floatChangers[index].propRef + " " + block.GetFloat(floatChangers[index].propRef));
                }
             }


             yield return null;
        }

        if(floatChangers[index].automaticlyPlayNext)
        {
            yield return new WaitForSeconds(floatChangers[index].waitTime);
            ChangeFloat(index + 1);
        }


        yield break;
    }

    public void PushFloat(int index, float strength)
    {
        // if(skinnedMesh) /// to do fix
        // {
        //     foreach (var item in skm)
        //     {
        //         if(!item.gameObject.activeInHierarchy){
        //         continue;
        //         }
        //     }
            
        // }
        // else{
        //     foreach (var item in rend)
        //     if(!item.gameObject.activeInHierarchy){
        //         continue;
        //         }
            
        //     }
            
        
        StartCoroutine(InterruptAndPushFloatR(index,strength));
    }

    IEnumerator InterruptAndPushFloatR(int index, float strength)
    {
        floatChangers[index].interrupted = true;
        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(PushFloatR(index, strength));
        yield break;
    }

    IEnumerator PushFloatR(int index, float strength)
    {
        floatChangers[index].interrupted = false;
        float timer = 0f;
        float startValue = block.GetFloat(floatChangers[index].propRef);
        float targetValue = startValue + (strength * floatChangers[index].pushFactor);

        while (timer <= floatChangers[index].duration)
        {
            timer += Time.unscaledDeltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, floatChangers[index].curve.Evaluate(timer/floatChangers[index].duration));
            block.SetFloat(floatChangers[index].propRef, newValue);
            
            if(skinnedMesh)
             {
                foreach (var item in skm)
                {
                    if(!item.gameObject.activeInHierarchy){
                    continue;}
                    
                    item.SetPropertyBlock(block);
                }
                
                
             }

             else
             {
                foreach (var item in rend)
                {
                    if(!item.gameObject.activeInHierarchy){
                    continue;}

                    item.SetPropertyBlock(block);
                    
                }
                
                 
             }

             yield return null;
        }


        yield break;
    }

    ////texture Changers

    public void SetRandomTexture()
    {
        int randomIndex = Random.Range(0, textureChangers.Length);
        SetTexture(randomIndex);
        
    }

    public void FadeInRandomTexture(float duration, AnimationCurve curve){
        int randomIndex = Random.Range(0, textureChangers.Length);
        FadeInTexture(randomIndex, duration, curve);
    }

    public void SetTexture(int index)
    {
        block.SetTexture(textureChangers[index].texPropRef, textureChangers[index].texture);
        if (skinnedMesh)
        {
            foreach (var item in skm)
            {
                item.SetPropertyBlock(block);
            }

        }

        else
        {
            foreach (var item in rend)
            {
                item.SetPropertyBlock(block);
            }
        }

        if(!usePropertyBlock)
             {
                
                //materialAsset.SetFloat(floatChangers[index].propRef, newValue);
                //print("Set Float: " + "Material: " + material.name + " " + floatChangers[index].propRef + " Value: " + block.GetFloat(floatChangers[index].propRef));
                
                if(skinnedMesh)
                {
                    
                    skm[0].sharedMaterial.SetTexture(textureChangers[index].texPropRef, block.GetTexture(textureChangers[index].texPropRef));
                }

                else
                {
                    rend[0].sharedMaterial.SetTexture(textureChangers[index].texPropRef, block.GetTexture(textureChangers[index].texPropRef));
                    //print(rend[0].sharedMaterial.name + " " + "Set Float " + floatChangers[index].propRef + " " + block.GetFloat(floatChangers[index].propRef));
                }
             }
    }

    public void FadeInTexture(int textureChangerIndex){
        float duration = Random.Range(textureChangers[textureChangerIndex].fadeDurationMin, textureChangers[textureChangerIndex].fadeDurationMax);
        StartCoroutine(InterruptAndFadeInTexture(textureChangers[textureChangerIndex].texture, duration, textureChangers[textureChangerIndex].curve));
    }

    public void FadeInTexture(int textureChangerIndex, float duration, AnimationCurve curve){
        
        StartCoroutine(InterruptAndFadeInTexture(textureChangers[textureChangerIndex].texture, duration, curve));
    }

    IEnumerator InterruptAndFadeInTexture(Texture texture, float duration, AnimationCurve curve){
        textureFader.isInterrupted = true;
        yield return new WaitForSeconds(0.01f);
        textureFader.isInterrupted = false;
        
        StartCoroutine(FadeInTextureR(texture, duration, curve));
        yield break;
    }

    IEnumerator FadeInTextureR(Texture texture, float duration, AnimationCurve curve){
        
        float startValue = block.GetFloat(textureFader.faderRef);
        float targetValue = textureFader.faderIsLeft ? 1f : 0f;
        string targetRef = textureFader.faderIsLeft ? textureFader.texRefB : textureFader.texRefA;
        
        float timer = 0f;
        bool fadeComplete = (textureFader.faderIsLeft && Mathf.Approximately(startValue, 0f) || !textureFader.faderIsLeft && Mathf.Approximately(startValue, 1f));
        if(fadeComplete){
            block.SetTexture(targetRef, texture);
        }
        
//        print("start fade loop: from " + startValue + "to " + targetValue + " , duration = " + duration + " " + texture.name);
        while (timer < duration && !textureFader.isInterrupted)
        {
            timer += Time.unscaledDeltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, curve.Evaluate(timer / duration));
            block.SetFloat(textureFader.faderRef, newValue);
            
            yield return null;
        }

        if(block.GetFloat(textureFader.faderRef) == 0f){
            textureFader.faderIsLeft = true;
            yield break;
        }
        if(block.GetFloat(textureFader.faderRef) == 1f){
            textureFader.faderIsLeft = false;
            yield break;
        }
        textureFader.faderIsLeft = !textureFader.faderIsLeft;
        yield break;

    }




    /// material changer

    public void SetRandomMaterial()
    {
        int randomIndex = Random.Range(0, materialChangers.Length);
        SetMaterial(randomIndex);
    }

    public void SetMaterial(int index)
    {
        if (skinnedMesh)
        {
            foreach (var item in skm)
            {
                Material[] meshMaterials = item.materials;
                meshMaterials[materialChangers[index].materialIndex] = materialChangers[index].material;
                item.materials = meshMaterials;
            }
            
        }

        else
        {
            foreach (var item in rend)
            {
                Material[] meshMaterials = item.materials;
                meshMaterials[materialChangers[index].materialIndex] = materialChangers[index].material;
                item.materials = meshMaterials;
            }
            
        }


      

        
    }

      /// Daumenkino

    public void PlayFloatDaumenkino(int index){
        StartCoroutine(InterruptAndPlayDaumenkino(index));
        
    }

    public void StopFloatDaumenkino(int index){
        floatChangers[index].playDaumenkino = false;
    }

    IEnumerator InterruptAndPlayDaumenkino(int index){
        floatChangers[index].playDaumenkino = false;
        yield return new WaitForSeconds(0.01f);
        floatChangers[index].playDaumenkino = true;
        StartCoroutine(PlayFloatDaumenkinoRoutine(index));
        yield break;

    }

    IEnumerator PlayFloatDaumenkinoRoutine(int index){

        float capturedTargetValue = floatChangers[index].targetValue;
        while (floatChangers[index].playDaumenkino)
        {
            float newValue = Random.Range(floatChangers[index].randomRangeMin, floatChangers[index].randomRangeMax);
            float waitTime = Random.Range(daumenkinoTimingMin, daumenkinoChangeMax);
            floatChangers[index].targetValue = newValue;
            ChangeFloat(index);
            yield return new WaitForSeconds(waitTime);
            
        }

        floatChangers[index].targetValue = capturedTargetValue;

        yield break;
    }

    


}