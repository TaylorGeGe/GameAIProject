﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState { 
    public List<Transform >path =new List<Transform>();

    private int index = 0;
 
    public PatrolState(FSMSystem fSMSystem):base(fSMSystem)
    { 
        stateID = StateID.Patrol; 
       GameObject  pathGameobject = GameObject.Find("Path");
        Transform[]  pathchildren = pathGameobject.GetComponentsInChildren<Transform>();
        foreach (Transform item in pathchildren)
        {
            if (item != pathGameobject.transform)
            { 
                path.Add(item);
            }
        } 
    }

    public override void Act(GameObject npc)
    { 
        npc.transform.LookAt(path[index].position);
        npc.transform.Translate(Vector3.forward * Time.deltaTime*3);
       if( Vector3.Distance(npc.transform.position, path[index].position) < 1)
        {
            index++;
            index %= path.Count;
        } 
    }

    public override void Reason(GameObject npc )
    {
        if (Vector3.Distance( CubeTranform.position,npc.transform.position)<3 )
        {
            FSMSysyem.PerformTransition(Transition.SeePlayer);
        }
    } 
}
