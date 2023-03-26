using UnityEngine;
using UnityEngine.VFX;

namespace ObliqueSenastions.VFXSpace
{

    [System.Serializable]
    public class VFXControl
    {

        public VisualEffect visualEffect;
        public VFXParameter[] vfxParameters;

        public float duration;

        

        [System.Serializable]
        public struct VFXParameter
        {
            public string parameterName;

            public AnimationCurve curve;

            public float valueMin;
            public float valueMax;

            public float currentValue;
        }
    }

}
