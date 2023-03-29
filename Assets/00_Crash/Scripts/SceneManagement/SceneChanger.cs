using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.TimelineSpace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObliqueSenastions.SceneSpace
{

    //public enum PortalToWhere { nextScene, previousScene, sceneByNumber, sceneByName };
    public enum PortalToWhere { nextScene, previousScene, sceneByNumber, sceneByName };
    public class SceneChanger : MonoBehaviour

    {

        [SerializeField] string transferSceneName;
        [SerializeField] bool useTransferScene = true;
        [SerializeField] string menueSceneName;
        [SerializeField] PortalToWhere portalToWhere;
        [SerializeField] int sceneByNumber = 1;
        [SerializeField] string sceneByName = "";



        [SerializeField] bool destroyAfterChange = true;

        [SerializeField] bool waitForTransferCondition = false;
        [SerializeField] string transferConditionTag;



        int currentSceneIndex;

        public void GoToNextScene()
        {
            portalToWhere = PortalToWhere.nextScene;

            ChangeScene();
        }

        public void GoToPreviousScene()
        {
            portalToWhere = PortalToWhere.previousScene;
            ChangeScene();
        }

        public void ChangeScene()
        {
            //       print("change scene");
            StartCoroutine(Transition());
        }

        public void ChangeScene(string name)
        {
            
            portalToWhere = PortalToWhere.sceneByName;
            sceneByName = name;
            StartCoroutine(Transition());
        }

        public void ChangeScene(int index)
        {
            portalToWhere = PortalToWhere.sceneByNumber;
            sceneByNumber = index;
            StartCoroutine(Transition());
        }

        public void GoToMenue()
        {
            portalToWhere = PortalToWhere.sceneByName;
            sceneByName = menueSceneName;
            GameObject timeline = GameObject.FindWithTag("Timeline");

            if (timeline != this.gameObject)
            {
                print("destroy Timeline");
                Destroy(timeline);
            }

            StartCoroutine(Transition());
        }

        public void QuitApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }







        private void Start()
        {
            // if(lookForColliderInChildren)
            // {
            //     triggerCollider = GetComponentInChildren<Collider>();
            //     triggerCollider.isTrigger = true;
            // }


            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        }



        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            //Safe Level
            // SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            // savingWrapper.Save();

            TimeLineHandler timeLineHandlerCurrentScene = GetComponent<TimeLineHandler>();

            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (useTransferScene && transferSceneName != "")
            {
                yield return SceneManager.LoadSceneAsync(transferSceneName);
            }




            // if(waitForTransferCondition){
            //     TransferCondition transferCondition = GameObject.FindWithTag(transferConditionTag).GetComponent<TransferCondition>();
            //     while (!transferCondition.GetTransferCondition())
            //     {
            //         yield return null;
            //     }
            // }

            if (portalToWhere == PortalToWhere.nextScene)
            {


                //            print("actual scene " + currentSceneIndex + "try loading next scene");
                if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    //                print("loading next scene: " + currentSceneIndex + 1 );
                    yield return SceneManager.LoadSceneAsync(currentSceneIndex + 1);
                }
                else
                {
                    print("There is no next Scene, Sorry!");
                }

            }

            if (portalToWhere == PortalToWhere.previousScene)
            {

                if (currentSceneIndex >= 1)
                {
                    yield return SceneManager.LoadSceneAsync(currentSceneIndex - 1);
                }

                else
                {
                    print("There is no previous Scene, Sorry!");
                }


            }

            if (portalToWhere == PortalToWhere.sceneByNumber)
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

            if (portalToWhere == PortalToWhere.sceneByName)
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



            // Restore Level
            // savingWrapper.Load();



            if (destroyAfterChange)
            {
                Destroy(gameObject);
            }


        }


    }

}
