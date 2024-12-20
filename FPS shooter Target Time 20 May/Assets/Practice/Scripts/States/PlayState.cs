using System.Collections;
using System.Collections.Generic;
using StateInterface;
using States;
using UnityEngine;

public class PlayState : IStateBase
{
    private StateManager stateManager;

    public PlayState(StateManager managerRefrence)
    {
        stateManager = managerRefrence;
        Debug.Log("Constructing PlayState");
    }
   
   public void StateUpdate()
   {
     if (Input.GetKeyDown(KeyCode.Space))
     {
        stateManager.SwitchState(new WonState(stateManager));
     }

     if (Input.GetKeyDown(KeyCode.Return))
     {
        stateManager.SwitchState(new LostState(stateManager));
     }
   }
}
