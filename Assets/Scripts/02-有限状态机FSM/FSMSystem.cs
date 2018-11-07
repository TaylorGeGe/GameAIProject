using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem {

    private Dictionary<StateID, FSMState> states=new Dictionary<StateID, FSMState>();
    public FSMState currentState;
    private StateID currentID;
   // public FSMState FSMStete { get { return currentState; } }

 
    public void Update(GameObject npc)
    {
        currentState.Act(npc);
        currentState.Reason(npc);
    }
     
    public void AddState(FSMState fSMStete)
    { 
        if (fSMStete==null)
        {
            Debug.Log("fSMStete为空 不能进行添加"); return;
        }
        if (currentState==null)
        {
           currentState = fSMStete;
            currentID = fSMStete.ID;
        }
        if (states.ContainsKey(fSMStete.ID))
        {
            Debug.Log("状态" + fSMStete.ID + "已经存在 不让重复添加"); return;
        }
       //  Debug.Log(fSMStete);
        states.Add(fSMStete.ID, fSMStete);
        
    }
    private void Delete(StateID id)
    {
        if (id ==StateID.NullStateID)
        {
            Debug.Log("fSMStete为空 不能进行添加"); return;
        }
        if (states.ContainsKey(id))
        {
            Debug.Log("无法删除一个不存在的ID：" + id); return;
        }
        states.Remove(id);
    }

    public void PerformTransition(Transition transition)
    {
        if (transition==Transition.NullTransition)
        {
            Debug.Log("无法执行空的转换条件NullTransition"); return;
        }

        StateID stateID = currentState.GetOutPutState(transition); 

        if (stateID==StateID.NullStateID)
        {
            Debug.Log("不需要发生转换"+ stateID ); return;
        }
        if (states.ContainsKey(stateID)==false)
        {
            Debug.Log("在状态机里面 不包含状态" + stateID+"无法进行转换"); return;
        } 
         FSMState  state = states[stateID];
        state.DOAdterLeaving(); 
        currentState = state;
        currentID = stateID;
        state.DoBeforeEntering(); 
    }


  
}
