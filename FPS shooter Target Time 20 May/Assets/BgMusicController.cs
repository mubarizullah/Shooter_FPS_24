using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BgMusicController : MonoBehaviour
{
   public AudioSource bgMusic;
   bool isSoundOn;
   public void Start()
   {
    isSoundOn = true;
   }

   public void Update()
   {
      if (isSoundOn)
      {
         isSoundOn = false;
         bgMusic.enabled = false;
      }
   }
   
}
