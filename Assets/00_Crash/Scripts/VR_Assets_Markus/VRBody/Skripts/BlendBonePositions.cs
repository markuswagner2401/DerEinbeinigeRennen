using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.AnimatorSpace
{

    public class BlendBonePositions : MonoBehaviour
    {
        [Tooltip("the visible Rig, that will be transformed, all three Rigs have to have the same number of bones with same names")]
        [SerializeField] Transform[] targetRig = null;
        [SerializeField] Transform[] sourceRigA = null;
        [SerializeField] Transform[] sourceRigB = null;

        Dictionary<Transform, Transform> targetsDictionary = new Dictionary<Transform, Transform>();

        Dictionary<Transform, Transform> sourcesDictionary = new Dictionary<Transform, Transform>();

        [SerializeField] float blendValue = 1f;

        [SerializeField] bool activateBlending = true;
        [SerializeField] float lerpToFixedValueDuration = 3f;
        [SerializeField] float fixedBlendValue = 0f;
        [SerializeField] AnimationCurve lerpToFixedValueCurve;

        [SerializeField] MapHandSpeed mapHandSpeed = null;

        BonesPair[] bonesPairs;

        struct BonesPair
        {
            public Transform sourceBoneA;
            public Transform sourceBoneB;
            public Transform targetBone;
        }


        void Start()
        {
            //CreateTargetsDictionary();
            bonesPairs = new BonesPair[sourceRigA.Length];

            if (mapHandSpeed == null)
            {
                mapHandSpeed = GetComponent<MapHandSpeed>();
            }

            CreateBonePairs();


        }
        private void Update()
        {
            if (activateBlending)
            {
                blendValue = CalculateBlendValue();
            }

        }


        void FixedUpdate()
        {
            if (bonesPairs.Length == 0) return;



            BlendPositions(blendValue);
        }


        public void ActivateBlending(bool value)
        {
            activateBlending = value;

            if (!activateBlending)
            {
                StartCoroutine(LerpToFixedBlendValue(lerpToFixedValueDuration, fixedBlendValue));
            }



        }

        IEnumerator LerpToFixedBlendValue(float duration, float newBlendValue)
        {
            float timer = 0f;
            float startValue = blendValue;
            while (timer <= duration)
            {
                timer += Time.deltaTime;

                blendValue = Mathf.Lerp(startValue, newBlendValue, lerpToFixedValueCurve.Evaluate(timer / duration));

                yield return null;
            }
            yield break;
        }

        private float CalculateBlendValue()
        {
            return Mathf.Clamp01(mapHandSpeed.GetMappedValueLeft() + mapHandSpeed.GetMappedValueRight());
        }

        private void CreateBonePairs()
        {
            if (sourceRigA == null || sourceRigB == null || targetRig == null) return;

            for (int i = 0; i < bonesPairs.Length; i++)
            {
                bonesPairs[i].sourceBoneA = sourceRigA[i];
                bonesPairs[i].sourceBoneB = sourceRigB[i];
                bonesPairs[i].targetBone = targetRig[i];
            }
        }



        private void BlendPositions(float currentBlendValue)
        {
            foreach (var bonesPair in bonesPairs)
            {
                bonesPair.targetBone.position = Vector3.Lerp(bonesPair.sourceBoneA.position, bonesPair.sourceBoneB.position, currentBlendValue);
                bonesPair.targetBone.rotation = Quaternion.Lerp(bonesPair.sourceBoneA.rotation, bonesPair.sourceBoneB.rotation, currentBlendValue);
                // bonesPair.Value.localScale = bonesPair.Key.localScale;

            }
        }

        private void LateUpdate()
        {

        }

        // private void CreateTargetsDictionary()
        // {
        //     if (sourceRigA == null || sourceRigB == null || targetRig == null) return;

        //     foreach (Transform bone in sourceRigA)
        //     {
        //         foreach (Transform targetBone in sourceRigB)
        //         {
        //             if (bone.name != targetBone.name) continue;

        //             targetsDictionary.Add(bone, targetBone);


        //         }
        //     }
        // }

        // private void CopyBonesPositions()
        // {
        //     foreach (var bonesPair in targetsDictionary)
        //     {
        //         bonesPair.Value.position = bonesPair.Key.position;
        //         bonesPair.Value.rotation = bonesPair.Key.rotation;
        //         // bonesPair.Value.localScale = bonesPair.Key.localScale;

        //     }
        // }
    }

}
