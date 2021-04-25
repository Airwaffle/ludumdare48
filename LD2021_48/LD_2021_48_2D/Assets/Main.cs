using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject hurtLetterPrefab;
    public Player player;
    public float restartDelay = 1.4f;
    public float fadeOutDelay = 2;
    public float fadeSpeed = 1;
    public float deathLetterInterval = 0.15f;

    public Image endBlack;
    public Text retries;
    public Text timeItTook;
    public Text text;

    private float timeBecomeDead = -1;
    private Canvas canvas;
    public float timeBecomeWon = -1;
    public float nextLetterTime = -1;
    public string sceneToReload = "SampleScene";

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectsOfType<Canvas>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsDead())
        {
            if (timeBecomeDead == -1)
                timeBecomeDead = Time.time;

            if (Time.time > nextLetterTime)
            {
                GameObject hurt = Instantiate(hurtLetterPrefab);
                hurt.transform.SetParent(canvas.transform);
                hurt.GetComponent<HurtScript>().Initialize();
                nextLetterTime = Time.time + deathLetterInterval;
            }

            if (Time.time - timeBecomeDead > restartDelay)
            {
                if (Input.anyKey)
                {
                    var globals = FindObjectsOfType<GlobalScript>();
                    
                    if (globals.Length > 0)
                        globals[0].retries++;

                    SceneManager.LoadScene(sceneToReload);
                }
            }
        }

        if (player.IsWon())
        {
            if (timeBecomeWon == -1)
            {
                timeBecomeWon = Time.time;
                retries.text = FindObjectsOfType<GlobalScript>()[0].retries.ToString();

                int seconds = (int)(Time.time - FindObjectsOfType<GlobalScript>()[0].startTime);
                int minutes = (int)Mathf.Floor(seconds / 60.0f);

                seconds -= minutes * 60;
                
                string secondsstring = seconds.ToString();
                if (seconds < 10)
                    secondsstring = "0" + secondsstring;
                    
                string minutesstring = minutes.ToString();
                if (minutes < 10)
                    minutesstring = "0" + minutesstring;

                timeItTook.text = minutesstring + ":" + secondsstring;
            }
            
            if (Time.time - timeBecomeWon > fadeOutDelay)
            {
                var blackColor = endBlack.color;
                blackColor.a += Time.deltaTime * fadeSpeed;
                endBlack.color = blackColor;

                var textColor = text.color;
                textColor.a += Time.deltaTime * fadeSpeed;
                retries.color = textColor;
                timeItTook.color = textColor;
                text.color = textColor;
            }
        }
    }
}
