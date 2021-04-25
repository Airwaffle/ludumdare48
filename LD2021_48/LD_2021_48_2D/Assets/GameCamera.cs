using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public AnimationCurve shakeCurve;
    public float shakeSpeed = 1;
    public float shakeIntensity = 1;

    public float topEdge = 37.25f;
    public Player player;

    private float shakeStartTime = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shake()
    {
        shakeStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = transform.position;
        newPos.y = player.transform.position.y;
        if (newPos.y > topEdge)
            newPos.y = topEdge;

        if (shakeStartTime != -1)
            newPos.y += shakeCurve.Evaluate(((Time.time-shakeStartTime)*shakeSpeed))*shakeIntensity;

        transform.position = newPos;
    }
}
