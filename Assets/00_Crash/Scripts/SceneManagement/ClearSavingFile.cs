using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace ObliqueSenastions.Saving
{

    public class ClearSavingFile : MonoBehaviour
    {


        void Start()
        {

        }

        public void ClearSaveFile()
        {
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Delete();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
