 using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
  using System;

 public class SoundCollider : MonoBehaviour
 {
     public GameObject textPrefab;
     public AudioClip clip;
     public float secondDelay;
     public string text;
     public bool makesPlayerWin = false;
     private AudioSource source;
     private Canvas canvas;

     private bool Played = false;

     void Start()   
     {
         source = GetComponent<AudioSource>(); 
         source.playOnAwake = false;
         source.clip = clip;
         canvas = FindObjectsOfType<Canvas>()[0];
     }        
 
    void OnTriggerEnter2D()    
     {
        if (Played)
            return;

        Played = true;
        
        StartCoroutine(ExecuteAfterTime(secondDelay, () =>
         {   
            source.Play();

            var obj = Instantiate(textPrefab);
            obj.GetComponent<Text>().text = text;
            obj.transform.SetParent(canvas.transform);

            if (makesPlayerWin)
            {
                FindObjectsOfType<Player>()[0].Win();
            }

         }));
     }


    private bool isCoroutineExecuting = false;
     IEnumerator ExecuteAfterTime(float time, Action task)
        {
            if (isCoroutineExecuting)
                yield break;
            isCoroutineExecuting = true;
            yield return new WaitForSeconds(time);
            task();
            isCoroutineExecuting = false;
        }
}
