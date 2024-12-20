using System.Collections;
using System.Collections.Generic;
using StateInterface;
using UnityEngine;

namespace States
{

public class LostState : IStateBase
{
    private StateManager stateManager;

    public LostState(StateManager managerRefrence)
    {
        stateManager = managerRefrence;
        Debug.Log("Constructing LostState");
    }
   
   public void StateUpdate()
   {
     if (Input.GetKeyDown(KeyCode.Space))
     {
        stateManager.SwitchState(new BeginState(stateManager));
     }
   }
}

}