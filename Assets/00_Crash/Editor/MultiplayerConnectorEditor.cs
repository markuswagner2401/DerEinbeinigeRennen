
using UnityEditor;
using UnityEngine;

namespace ObliqueSenastions.PunNetworking
{
    [CustomEditor(typeof(MultiplayerConnector))]
    public class MultiplayerConnectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MultiplayerConnector connector = (MultiplayerConnector)target;

            if (GUILayout.Button("Call Disconnect RPC"))
            {
                bool confirm = EditorUtility.DisplayDialog("Disconnect All", "Are you sure you want to disconnect all clients?", "Yes", "Cancel");

                if (confirm)
                {
                    connector.CallDisconnectRPC();
                }
            }
        }
    }

}

