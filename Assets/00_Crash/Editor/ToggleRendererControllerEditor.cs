using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace ObliqueSenastions.TransformControl
{
    [CustomEditor(typeof(ToggleRendererController))]
    public class ToggleRendererControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ToggleRendererController controller = (ToggleRendererController)target;

            if (GUILayout.Button("Toggle Renderer"))
            {
                ToggleRenderers(controller.gameObject);
            }

            if (GUILayout.Button("Toggle Second Childs"))
            {
                ToggleSecondChilds(controller.gameObject);
            }
        }

        private void ToggleRenderers(GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = !renderer.enabled;
            }
        }

        private void ToggleSecondChilds(GameObject obj)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                Transform firstChild = obj.transform.GetChild(i);
                for (int j = 0; j < firstChild.childCount; j++)
                {
                    Transform secondChild = firstChild.GetChild(j);
                    secondChild.gameObject.SetActive(!secondChild.gameObject.activeSelf);
                }
            }
        }
    }

}

