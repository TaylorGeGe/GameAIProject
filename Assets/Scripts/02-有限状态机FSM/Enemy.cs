using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private FSMSystem fSMSystem; 
	void Start () {
        InitFsmSystem(); 
    } 
	void Update () {
        fSMSystem.Update(this.gameObject); 
    }
    void InitFsmSystem()
    {
        fSMSystem = new FSMSystem();
        FSMState patrolState = new PatrolState(fSMSystem);
        patrolState.AddTransition(Transition.SeePlayer, StateID.Chease);
        fSMSystem.AddState(patrolState);
        FSMState chaseState = new ChaseState(fSMSystem);
        chaseState.AddTransition(Transition.LosePlayer, StateID.Patrol); 
        fSMSystem.AddState(chaseState);

    }
}
