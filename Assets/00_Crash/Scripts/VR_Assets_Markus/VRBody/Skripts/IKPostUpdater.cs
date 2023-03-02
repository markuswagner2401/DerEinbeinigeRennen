using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class IKPostUpdater : MonoBehaviour
{
    	public IK ik;
	    public ParticleSystem p;
	 
	    private void Start()
	    {
	        ik.GetIKSolver().OnPostUpdate += OnPostUpdate;
	    }
	 
	    private void OnPostUpdate()
	    {
	        if (!enabled) return;
			if(p == null) return;
	        p.Simulate(Time.deltaTime, true, false);
    }

}
