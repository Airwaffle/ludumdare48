using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMusicCollider : MonoBehaviour
{
    public AudioSource musicPlayer;
    public AudioClip newClip;

     void OnTriggerEnter2D()    
     {
         if (newClip != null)
         {
        musicPlayer.clip = newClip;
        musicPlayer.Play();
        }
        else 
        {
            musicPlayer.Stop();
        }
     }
}
