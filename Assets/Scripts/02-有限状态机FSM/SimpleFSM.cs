using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFSM : MonoBehaviour {
    InputField field;
  
    public  enum State
    { 
        Idel,//闲置
        Patrol,//巡逻
        Chase,//追
        Attack,//打
    }   
    private void ProcessStateIdle()
    {

    }
    private void ProcessStatePatrol()
    {

    }
    private void ProcessStateChase()
    {

    }
    private void ProcessStateAttack()
    {

    }
    private State state = State.Idel;
    void Update () {
        switch (state)
        {
            case State.Idel:
                ProcessStateIdle();
                break;
            case State.Patrol:
                ProcessStatePatrol();
                break;
            case State.Chase:
                ProcessStateChase();
                break;
            case State.Attack:
                ProcessStateAttack();
                break;
            default:
                break;
        }
    }
}
