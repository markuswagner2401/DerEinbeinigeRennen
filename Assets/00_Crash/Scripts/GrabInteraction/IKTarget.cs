using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTarget : MonoBehaviour
{
    [SerializeField] IKTargetChanger iKTargetChanger;

    public void ChangeAimIKTarget()
    {
        if (iKTargetChanger == null) return;
        iKTargetChanger.SetAimIKTarget(this.transform);

    }
}
