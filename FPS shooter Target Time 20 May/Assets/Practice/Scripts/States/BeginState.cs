using System.Collections;
using System.Collections.Generic;
using StateInterface;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace States{
public class BeginState : IStateBase
{
   private StateManager stateManager;
   public BeginState(StateManager managerRefrence)
   {
    stateManager = managerRefrence;
    Debug.Log("Constructing BeginState");
   }

   public void StateUpdate()
   {
     if (Input.GetKeyDown(KeyCode.Space))
     {
        stateManager.SwitchState(new PlayState(stateManager));
     }
   }
}
}
