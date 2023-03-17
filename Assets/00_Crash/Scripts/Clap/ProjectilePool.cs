using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.ClapSpace
{
    public class ProjectilePool : MonoBehaviour
    {

        public static ProjectilePool instance;

        

        [SerializeField] Projectile[] projectiles;

        [System.Serializable]
        struct Projectile
        {
            public string note;
            public GameObject projectilePrefab;
            public int poolSize;
            public GameObject[] pool;
            public int currentPoolIndex;





            public float deactivateAfter;


        }

        //public GameObject projectilePrefab;
        //public int poolSize = 100;

        GameObject[] pool;
        int currentPoolIndex = 0;

        private void Awake()
        {
            
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            else
            {
                instance = this;
            }

            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i].pool = new GameObject[projectiles[i].poolSize];

                projectiles[i].currentPoolIndex = 0;

                for (int j = 0; j < projectiles[i].poolSize; j++)
                {
                    projectiles[i].pool[j] = Instantiate(projectiles[i].projectilePrefab, instance.transform) as GameObject;
                    projectiles[i].pool[j].SetActive(false);

                }
            }



            //

            // pool = new GameObject[poolSize];

            // for (int i = 0; i < poolSize; i++)
            // {
            //     pool[i] = Instantiate(projectilePrefab, instance.transform) as GameObject;
            //     pool[i].SetActive(false);

            // }

        }



        public static GameObject Take(Vector3 position, Quaternion rotation)
        {
            int index = 0;

            if (++instance.projectiles[index].currentPoolIndex >= instance.projectiles[index].pool.Length)
            {
                //instance.projectiles[index].retaking = true;
                instance.projectiles[index].currentPoolIndex = 0;
            }

            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].SetActive(false);



            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].transform.position = position;
            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].transform.rotation = rotation;

            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].SetActive(true);

            return instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex];

        }

        public static GameObject Take(int index, Vector3 position, Quaternion rotation)
        {


            if (++instance.projectiles[index].currentPoolIndex >= instance.projectiles[index].pool.Length)
            {
                return null;
                //instance.projectiles[index].currentPoolIndex = 0;
            }

            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].SetActive(false);
            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].transform.position = position;
            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].transform.rotation = rotation;
            instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex].SetActive(true);
            // if(instance.projectiles[index].deactivateAfter > 0 ){
            //     instance.StartCoroutine(DeactivationRoutine(instance.projectiles[index].deactivateAfter, instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex]));
            // }

            return instance.projectiles[index].pool[instance.projectiles[index].currentPoolIndex];

        }

        public static void DeactivateAll()
        {
            for (int i = 0; i < instance.projectiles.Length; i++)
            {
                for (int j = 0; j < instance.projectiles[i].pool.Length; j++)
                {
                    instance.projectiles[i].pool[j].SetActive(false);
                }
            }
        }

        // static IEnumerator DeactivationRoutine(float time, GameObject go){
        //     yield return new WaitForSeconds(time);
        //     go.SetActive(false);
        //     yield break;
        // }







    }

}


