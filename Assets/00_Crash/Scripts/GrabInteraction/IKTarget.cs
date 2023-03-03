using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObliqueSenastions.FinalIKControl
{
    public class IKTarget : MonoBehaviour
    {
        [SerializeField] IKTargetChanger iKTargetChanger;

        public void ChangeAimIKTarget()
        {
            if (iKTargetChanger == null) return;
            iKTargetChanger.SetAimIKTarget(this.transform);

        }
    }


}
