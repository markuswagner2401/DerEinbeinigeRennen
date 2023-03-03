using System.Collections;
using System.Collections.Generic;
using ObliqueSenastions.Looping;
using UnityEngine;

namespace ObliqueSenastions.TransformControl
{

    public class ProjectileJoker : MonoBehaviour
    {
        TeleportProjectile[] currentProjectiles;

        [SerializeField] bool shrink = true;
        [SerializeField] float minScaleMin = 0.1f;
        [SerializeField] float minScaleMax = 0.5f;

        [SerializeField] float shrinkSpeedMin = 0.4f;
        [SerializeField] float shrinkSpeedMax = 1f;

        [SerializeField] bool destroy = false;

        void Start()
        {
            currentProjectiles = FindObjectsOfType<TeleportProjectile>();
            if (shrink)
            {
                foreach (var projectile in currentProjectiles)
                {
                    if (projectile.transform.gameObject == this.gameObject) continue;
                    StartCoroutine(ShrinkObject(projectile));
                }



            }
        }

        private IEnumerator ShrinkObject(TeleportProjectile projectile)
        {
            float shrinkSpeed = Random.Range(shrinkSpeedMin, shrinkSpeedMax);
            float scaleMin = Random.Range(minScaleMin, minScaleMax);


            while (projectile.transform.localScale.x > minScaleMin)
            {
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x - (shrinkSpeedMax * Time.deltaTime),
                                                                projectile.transform.localScale.y - (shrinkSpeedMax * Time.deltaTime),
                                                                projectile.transform.localScale.z - (shrinkSpeedMax * Time.deltaTime));

                yield return null;
            }
            if (destroy)
            {
                Destroy(projectile.transform.gameObject);
            }

            yield break;
        }



        void Update()
        {

        }
    }

}
