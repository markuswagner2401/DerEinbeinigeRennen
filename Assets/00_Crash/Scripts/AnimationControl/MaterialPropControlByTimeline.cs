using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.TimelineSpace;

using UnityEngine;

namespace ObliqueSenastions.MaterialControl

{
    public class MaterialPropControlByTimeline : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer = null;
        [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer = null;

        [SerializeField] bool useSmr = false;
        [SerializeField] MaterialControl[] controls;
        [System.Serializable]
        struct MaterialControl
        {
            [Tooltip("name has to match with name in FloatControlByTimeline on Timeline")]
            public string name;
            public string propRef;

            // [Tooltip("Has to match with element in FloatControlByTimeline on Timeline")]
            // public int indexAtTL;

            // public bool setValueAtStart;
            public float valueAtStart;
        }

        private MaterialPropertyBlock block = null;

        FloatControlByTimeline floatControlByTimeline = null;

        private void OnEnable()
        {
            block = new MaterialPropertyBlock();
        }
        void Start()
        {
            if (floatControlByTimeline == null)
            {
                floatControlByTimeline = TimeLineHandler.instance.GetComponent<FloatControlByTimeline>();
            }


            foreach (var control in controls)
            {
                block.SetFloat(control.propRef, control.valueAtStart);
                int index = floatControlByTimeline.GetParameterIndexByName(control.name);
                floatControlByTimeline.SetStartValue(index, block.GetFloat(control.propRef));
            }
        }


        void Update()
        {
            if (floatControlByTimeline == null)
            {
                floatControlByTimeline = TimeLineHandler.instance.GetComponent<FloatControlByTimeline>();
            }

            foreach (var control in controls)
            {
                float newValue = floatControlByTimeline.GetValue(control.name);
                block.SetFloat(control.propRef, newValue);
            }

            if (useSmr)
            {
                if (skinnedMeshRenderer == null) return;
                skinnedMeshRenderer.SetPropertyBlock(block);
            }

            else
            {
                if (meshRenderer == null) return;
                meshRenderer.SetPropertyBlock(block);
            }


        }

        public MaterialPropertyBlock GetBlock()
        {
            return block;
        }


    }

}


