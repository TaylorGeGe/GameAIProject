using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition=0,
    SeePlayer,
    LosePlayer,
}
 public  enum StateID
{
    NullStateID=0,
    Patrol,//巡逻
    Chease,//跟踪
}
 
public abstract class FSMState  {

    public Transform CubeTranform;
   
   protected  FSMSystem FSMSysyem; 
    protected FSMState(FSMSystem FSMSysyem)
    {
        CubeTranform = GameObject.Find("Cube").transform;
        this.FSMSysyem = FSMSysyem; 
    } 
    protected StateID stateID;

    public StateID ID { get { return stateID; } }

    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    
    public  void AddTransition(Transition transition,StateID stateID)
    {
        if (transition==Transition.NullTransition)
        {
            Debug.Log("NullTransition为"); return;
        }
        if (stateID==StateID.NullStateID)
        {
            Debug.Log("NullStateID"); return;
        }
        if (map.ContainsKey(transition))
        {
            Debug.Log("Map 已经存在了transition:" + transition); return;
        }
       // Debug.Log("map  "+ transition+"  "+ stateID);
        map.Add(transition, stateID);
    }
    public void RemoveTransition(Transition transition )
    {
        if (transition == Transition.NullTransition)
        {
            Debug.Log("NullTransition为"); return;
        }
        if (stateID == StateID.NullStateID)
        {
            Debug.Log("NullStateID"); return;
        }
        if (map.ContainsKey(transition)==false)
        {
            Debug.Log("transition不存在当前map中");
        }
        map.Remove(transition);
    }

    public StateID GetOutPutState(Transition transition)
    {
        if (map.ContainsKey(transition)==false)
        {
            Debug.Log("没有要转变的状态");
            return StateID.NullStateID;
        } 
        return map[transition]; 
    }

    public virtual void DoBeforeEntering() { }

    public virtual void DOAdterLeaving() { }

    public abstract void Act(GameObject npc);
    public abstract void Reason(GameObject npc); //判断转换条件


}
