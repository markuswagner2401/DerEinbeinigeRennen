using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.UISpace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace ObliqueSenastions.SceneSpace
{

    public enum GameMode
    {
        vr,
        desktop
    }
    public class MultiplayerDeviceChecker : MonoBehaviour
    {


        // [SerializeField] UnityEvent onHMDActiveAtStart;
        // [SerializeField] UnityEvent onHMDNotActiveAtStart;

        GameMode clientsGameMode;

        void Start()
        {


            if (XRSettings.isDeviceActive)
            {
                //onHMDActiveAtStart.Invoke();

                clientsGameMode = GameMode.vr;


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
            while (!done)
            {
                if (TryGetComponent<UIConnectorActivator>(out UIConnectorActivator connectorActivator))
                {
                    done = true;
                    connectorActivator.ShowConnector(value);
                    yield return null;
                }

                yield return null;
            }

            Debug.Log("Show Connect As Inspizient because no Headset found");

            yield break;
        }




    }

}
