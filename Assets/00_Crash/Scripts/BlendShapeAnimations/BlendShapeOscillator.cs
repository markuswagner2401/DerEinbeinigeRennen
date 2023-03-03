
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.Animation
{
    public class BlendShapeOscillator : MonoBehaviour
    {
        [SerializeField] float timeMax = 3f;
        [SerializeField] float timeMin = 0.1f;

        [SerializeField] float blendShapeWeightAddition = 0.1f;


        SkinnedMeshRenderer skinnedMesh;
        float startBlendShapeWeight;

        // Start is called before the first frame update
        void Start()
        {
            skinnedMesh = GetComponent<SkinnedMeshRenderer>();


            startBlendShapeWeight = skinnedMesh.GetBlendShapeWeight(0);


            StartCoroutine(OscillateBlendShape());

        }



        private IEnumerator OscillateBlendShape()
        {
            while (true)
            {
                skinnedMesh.SetBlendShapeWeight(0, UnityEngine.Random.Range(startBlendShapeWeight, startBlendShapeWeight + blendShapeWeightAddition));
                yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
            }

        }
    }


}
