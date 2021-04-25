using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class TextDialog : MonoBehaviour
{
    public AnimationCurve xMoveCurve;
    public AnimationCurve fadeCurve;

    public Text text;
    
    private float startTime;

    void Awake()
    {   
        startTime = Time.time;
    }

    void Setup()
    {   
    }

    void Update()
    {
        var pos = transform.position;
        pos.x = xMoveCurve.Evaluate((Time.time-startTime)*0.15f)*600 + 300;
        transform.position = pos;

        var newColor = text.color;
        newColor.a = 0;

        text.color = Color.Lerp(text.color , newColor, 0.5f * Time.deltaTime);
        //var color = text.color;
        //color.a = 255*fadeCurve.Evaluate((Time.time-startTime));
        //Debug.Log(fadeCurve.Evaluate((Time.time-startTime)));
        //text.color = color;

    }
}
