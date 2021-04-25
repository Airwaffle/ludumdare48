using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour 
{
    public static GlobalScript Instance;

    public int retries = 0;
    public float startTime = 0;

    void Awake()   
    {
        if (Instance == null)
        {
            startTime = Time.time;
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}