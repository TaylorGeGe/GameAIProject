using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState :FSMState {
 
    public ChaseState(FSMSystem fSMSystem):base(fSMSystem)
    {
        stateID = StateID.Chease;

    }

    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(CubeTranform);

        npc.transform.Translate(Vector3.forward * 2 * Time.deltaTime);

    }

    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(CubeTranform.position, npc.transform.position) >5)
        {
            FSMSysyem.PerformTransition(Transition.LosePlayer);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
