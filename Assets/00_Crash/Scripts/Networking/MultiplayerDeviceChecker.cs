using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Photon.Pun;


namespace ObliqueSenastions.SceneSpace
{
    

    public enum GameMode
    {
        vr,
        desktop
    }
    [RequireComponent(typeof(UIConnectorActivator))]
    public class MultiplayerDeviceChecker : MonoBehaviour
    {
        float timer;


        // [SerializeField] UnityEvent onHMDActiveAtStart;
        // [SerializeField] UnityEvent onHMDNotActiveAtStart;

        GameMode clientsGameMode;

        void Start()
        {
            if(PhotonNetwork.IsConnected)
            {
                StartCoroutine(ActivateUIR(false));
                return;
            }

            if (XRSettings.isDeviceActive)
            {
                //onHMDActiveAtStart.Invoke();

                clientsGameMode = GameMode.vr;

                StartCoroutine(ActivateUIR(false));


                print("hmd acitve");
            }

            else
            {


                clientsGameMode = GameMode.desktop;

                StartCoroutine(ActivateUIR(true));

                print("hmd not active");
            }

        }

        IEnumerator ActivateUIR(bool value)
        {
           

            bool done = false;
            while (!done && timer < 10f)
            {
                timer += Time.deltaTime;

                if (TryGetComponent<UIConnectorActivator>(out UIConnectorActivator connectorActivator))
                {
                    done = true;
                    connectorActivator.ShowConnector(value);
                    yield break;
                }

                Debug.LogError("MultiplayerDeviceChecker trying to find UIConnectorActivator");

                yield return null;
            }

            Debug.Log("Show Connect As Inspizient because no Headset found");

            yield break;
        }




    }

}
