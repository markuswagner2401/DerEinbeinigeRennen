using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChanger : MonoBehaviour
{
    [SerializeField] ObjectTag[] objectTags;




    [System.Serializable]
    struct ObjectTag
    {
        public GameObject gameObject;
        public string capturedTag;
        public string newTag;
    }


    
    void Start()
    {
        for (int i = 0; i < objectTags.Length; i++)
        {
            objectTags[i].capturedTag = objectTags[i].gameObject.tag;
        }
    }

    
    

    public void ChangeTag(string tag)
    {
        for (int i = 0; i < objectTags.Length; i++)
        {
            objectTags[i].newTag = tag;
            objectTags[i].gameObject.tag = tag;
        }
    }

    public void ChangeTagBack()
    {
        for (int i = 0; i < objectTags.Length; i++)
        {
            objectTags[i].gameObject.tag = objectTags[i].capturedTag;
        }
    }
}
