using UnityEditor;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{
    [CustomEditor(typeof(ObjectSpawner))]
    public class ObjectSpawnerEditor : Editor
    {
        private bool isEditable = true;
        private bool renderersEnabled = true;
        private ObjectSpawner spawner;

        private void OnEnable()
        {
            spawner = (ObjectSpawner)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Toggle Edit Mode"))
            {
                isEditable = !isEditable;

                if (!isEditable)
                {
                    SpawnObjects();
                }
                else
                {
                    ClearObjects();
                }
            }

            if (GUILayout.Button("Toggle Renderers"))
            {
                ToggleRenderers();
            }

            if (Application.isPlaying)
            {
                GUI.enabled = false;
            }




        }

        private void SpawnObjects()
        {
            if (spawner.targetTransforms == null || spawner.objectsToSpawn == null || spawner.objectsToSpawn.Length == 0) return;

            spawner.spawnedObjects = new GameObject[spawner.targetTransforms.Length];
            int cycleIndex = 0;

            for (int i = 0; i < spawner.targetTransforms.Length; i++)
            {
                if (spawner.targetTransforms[i] != null)
                {
                    Vector3 position = spawner.targetTransforms[i].position + spawner.positionOffset;
                    Quaternion rotation = spawner.targetTransforms[i].rotation * Quaternion.Euler(spawner.rotationOffset);

                    GameObject objectToSpawn;

                    switch (spawner.spawnMode)
                    {
                        case ObjectSpawner.SpawnMode.Cycle:
                            objectToSpawn = spawner.objectsToSpawn[cycleIndex % spawner.objectsToSpawn.Length];
                            cycleIndex++;
                            break;
                        case ObjectSpawner.SpawnMode.Random:
                            objectToSpawn = spawner.objectsToSpawn[Random.Range(0, spawner.objectsToSpawn.Length)];
                            break;
                        default:
                            objectToSpawn = spawner.objectsToSpawn[0];
                            break;
                    }

                    spawner.spawnedObjects[i] = Instantiate(objectToSpawn, position, rotation);
                    spawner.spawnedObjects[i].transform.SetParent(spawner.targetTransforms[i]);
                }
            }
        }

        private void ClearObjects()
        {
            if (spawner.spawnedObjects == null) return;

            for (int i = 0; i < spawner.spawnedObjects.Length; i++)
            {
                if (spawner.spawnedObjects[i] != null)
                {
                    DestroyImmediate(spawner.spawnedObjects[i]);
                }
            }
        }

        private void ToggleRenderers()
        {
            if (spawner.targetTransforms == null) return;

            renderersEnabled = !renderersEnabled;

            foreach (Transform targetTransform in spawner.targetTransforms)
            {
                if (targetTransform != null)
                {
                    Renderer[] renderers = targetTransform.GetComponentsInChildren<Renderer>(true);
                    foreach (Renderer renderer in renderers)
                    {
                        renderer.enabled = renderersEnabled;
                    }
                }
            }
        }
    }

}

