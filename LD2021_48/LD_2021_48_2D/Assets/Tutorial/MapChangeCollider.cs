using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MapChangeCollider : MonoBehaviour
{
    public string nextMap = "SampleScene";

     void OnTriggerEnter2D()    
     {
        SceneManager.LoadScene(nextMap);
     }
}
