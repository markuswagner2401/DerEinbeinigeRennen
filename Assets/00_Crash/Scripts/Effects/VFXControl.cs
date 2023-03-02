using UnityEngine.VFX;

[System.Serializable]
public class VFXControl 
{

    public VisualEffect visualEffect;
    public VFXParameter[] vfxParameters;

    [System.Serializable]
    public struct VFXParameter
    {
        public string parameterName;

        public float valueMin;
        public float valueMax;

        public float currentValue;
    }
}