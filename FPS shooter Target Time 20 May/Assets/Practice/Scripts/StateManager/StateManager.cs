using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using StateInterface;
using UnityEditor.Timeline.Actions;


public class StateManager : MonoBehaviour
{
   public IStateBase activeState;

   void Start()
{
    activeState = new BeginState(this);
}

   void Update()
   {
    if (activeState != null)
    {
        activeState.StateUpdate();
    }
   }

   public void SwitchState(IStateBase newState)
   {
     activeState = newState;
   }
}
