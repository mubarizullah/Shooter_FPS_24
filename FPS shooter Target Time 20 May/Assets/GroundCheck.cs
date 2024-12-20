using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class GroundCheck : MonoBehaviour 
{
  public Player player;
  void Start()
  {
    player = GetComponent<Player>();
  }
       void OnCollisionStay(Collision collision)
   {
        if (collision.gameObject.CompareTag("Floor"))
    {
        Debug.Log("collision method working test");
        player.isGrounded = true;
    }
   }
   
}
