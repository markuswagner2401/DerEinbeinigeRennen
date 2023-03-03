using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ObliqueSenastions.SceneSpace
{

    public class SceneLoader : MonoBehaviour

    {
        [SerializeField] Collider triggerCollider = null;
        [SerializeField] int transferScene = 0;
        [SerializeField] PortalToWhere portalToWhere;
        [SerializeField] int sceneByNumber = 1;
        [SerializeField] string sceneByName = "";
        [SerializeField] bool lookForColliderInChildren = true;
        [SerializeField] string triggerTag = "Player";



        int currentSceneIndex;

        public void ChangeScene()
        {
            StartCoroutine(Transition());
        }



        private void Start()
        {
            if (lookForColliderInChildren)
            {
                triggerCollider = GetComponentInChildren<Collider>();
                triggerCollider.isTrigger = true;
            }


            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == triggerTag)
            {
                print("triggered by player");
                StartCoroutine(Transition());
            }

        }

        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            //Safe Level
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();


            yield return SceneManager.LoadSceneAsync(transferScene);

            if (portalToWhere == PortalToWhere.nextScene)
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

            if (portalToWhere == PortalToWhere.previousScene)
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
            savingWrapper.Load();




            Destroy(gameObject);

        }


    }

}
