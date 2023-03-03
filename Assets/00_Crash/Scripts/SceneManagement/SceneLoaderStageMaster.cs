using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Saving;
using ObliqueSenastions.TimelineSpace;
using ObliqueSenastions.UISpace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObliqueSenastions.StageMasterSpace
{

    public enum PortalTo { nextScene, previousScene, sceneByNumber, sceneByName };

    public class SceneLoaderStageMaster : MonoBehaviour

    {

        [SerializeField] int transferScene = 0;
        [SerializeField] PortalTo portalTo;
        [SerializeField] int sceneByNumber = 1;
        [SerializeField] string sceneByName = "";



        [SerializeField] string destoyAfterScene = "";
        [SerializeField] bool saveStats = false;





        int currentSceneIndex;

        public void ChangeScene()
        {
            StartCoroutine(Transition());
        }



        private void Start()
        {



        }



        IEnumerator Transition()
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            string currentSceneName = SceneManager.GetActiveScene().name;

            DontDestroyOnLoad(gameObject);

            //Safe Level

            if (saveStats)
            {
                SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
                savingWrapper.Save();

            }



            yield return SceneManager.LoadSceneAsync(transferScene);




            if (portalTo == PortalTo.previousScene)
            {

                if (currentSceneIndex > 1) // 1 ist kleinste szene da 0 Übergangsszene
                {
                    yield return SceneManager.LoadSceneAsync(currentSceneIndex - 1);
                }

                else
                {
                    print("There is no previous Scene, Sorry!");
                }


            }

            if (portalTo == PortalTo.nextScene)
            {


                print("actual scene " + currentSceneIndex + "try loading next scene");
                if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    print("loading next scene: " + currentSceneIndex + 1);
                    yield return SceneManager.LoadSceneAsync(currentSceneIndex + 1);
                }
                else
                {
                    print("There is no next Scene, Sorry!");
                }

            }

            if (portalTo == PortalTo.sceneByNumber)
            {
                if (SceneManager.sceneCountInBuildSettings >= sceneByNumber)
                {
                    yield return SceneManager.LoadSceneAsync(sceneByNumber);
                }

                else
                {
                    print("there is no scene with this number");
                }

            }

            if (portalTo == PortalTo.sceneByName)
            {
                if (SceneManager.GetSceneByName(sceneByName) != null)
                {
                    yield return SceneManager.LoadSceneAsync(sceneByName);
                }

                else
                {
                    print("There is no scene with this name, sorry");
                }

            }

            print("scene loaded");

            // Restore Level

            if (saveStats)
            {
                SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
                savingWrapper.Load();
            }

            if (destoyAfterScene == currentSceneName)
            {
                Destroy(gameObject);
                yield break;
            }



            // Restore Stage Master

            GetComponent<PersitentTimelineStarter>().RestoreStageMaster();

            GetComponent<UITextStreamer>().ReinitializeTextUI();



        }


    }

}
