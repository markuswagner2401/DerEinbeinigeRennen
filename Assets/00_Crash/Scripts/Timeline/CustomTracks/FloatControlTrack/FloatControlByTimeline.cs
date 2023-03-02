using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatControlByTimeline : MonoBehaviour
{

    [SerializeField] float currentValue;    
    [SerializeField] string currentParameterName;
    [SerializeField] FloatControllers[] floatControllers;
    [System.Serializable]
    struct FloatControllers
    {
        public string name;

        [Tooltip("hint to the reading component, only for scene navigation")]
        public Transform referenceToReader;
        public float currentValue;

        public float startValue;

        [Tooltip("if false, the value above will be used as start value")]
        public bool receiveStartValueByComponent;

        [HideInInspector]
        public bool initialized;

        
    }

    Dictionary<string, int> lookup = new Dictionary<string, int>();

    private void Awake() 
    {
        for (int i = 0; i < floatControllers.Length; i++)
        {
            if(floatControllers[i].receiveStartValueByComponent)
            {
                floatControllers[i].initialized = false;
            }

            else
            {
                floatControllers[i].initialized = true;
            }
        }

        SetupLookup();
    }

    // Get by PlayableBehaviour (once per track)


    public int GetParameterIndexByName(string name) 
    {
        for (int i = 0; i < floatControllers.Length; i++)
        {
            if(name == floatControllers[i].name)
            {
                return i;
            }
        }

  //      Debug.Log("FloatControlByTimeline: No Index found with name: " + name);
        return -1;
    }

    public void SetupLookup()
    {
        lookup.Clear();

        for (int i = 0; i < floatControllers.Length; i++)
        {
            lookup.Add(floatControllers[i].name, i);
        }
    }

    public void SetValue(string parameterName, float newValue)
    {
        if(!lookup.ContainsKey(parameterName)) return;
        SetValue(lookup[parameterName], newValue);
    }

    private void SetValue(int parameterIndex, float newValue) // By PlayableBehaviour (Mixer)
    {
        if(!floatControllers[parameterIndex].initialized)
        {
            
            return;
        } 
        currentParameterName = floatControllers[parameterIndex].name; 
        currentValue = newValue;  
        floatControllers[parameterIndex].currentValue = newValue;
    }

    public float GetValue(int parameterIndex) // Get By Scene Component
    {
        return floatControllers[parameterIndex].currentValue;
    }

    public float GetValue(string parameterName)
    {
        int index = lookup[parameterName];
        return GetValue(index);
    }

   

    public void SetStartValue(int parameterIndex, float value) // By Scene Component
    {
        
        floatControllers[parameterIndex].startValue = value;
        floatControllers[parameterIndex].initialized = true;
    }

    public float GetStartValue(string parameterName)
    {
        if(!lookup.ContainsKey(parameterName)) return -1;
        return GetStartValue(lookup[parameterName]);
    }

    private float GetStartValue(int parameterIndex) // By PlayableBehaviour (Mixer)
    {
        return floatControllers[parameterIndex].startValue;
    }
   
}
