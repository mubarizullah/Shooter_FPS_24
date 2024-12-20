using System.Collections;
using System.Collections.Generic;
using StateInterface;
using States;
using UnityEngine;

public class WonState : IStateBase
{

    private StateManager stateManager;

    public WonState(StateManager managerRefrence)
    {
        stateManager = managerRefrence;
        Debug.Log("Constructing WonState");
    }
   
   public void StateUpdate()
   {
    if (Input.GetKeyDown(KeyCode.Space))
    {
        stateManager.SwitchState(new BeginState(stateManager));
    }
   }
}
