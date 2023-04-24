
using UnityEditor;
using UnityEngine;

namespace ObliqueSenastions.AudioControl
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SoundManager soundManager = (SoundManager)target;

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play Test Sound"))
            {
                soundManager.PlaySound(soundManager.testSound);
            }
            if (GUILayout.Button("Stop Test Sound"))
            {
                soundManager.StopSound(soundManager.testSound);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Hold On"))
            {
                soundManager.HoldOn(true);
            }
            if (GUILayout.Button("Go On"))
            {
                soundManager.HoldOn(false);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}


