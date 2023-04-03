using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    [CustomEditor(typeof(ObstacleAnimator))]
    public class ObstacleAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ObstacleAnimator obstacleAnimator = (ObstacleAnimator)target;
            if (GUILayout.Button("Trigger OnTriggerEnter"))
            {
                obstacleAnimator.DebugTriggerOnEnter();
            }
        }
    }
}

