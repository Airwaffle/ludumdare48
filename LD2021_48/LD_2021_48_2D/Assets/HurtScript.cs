using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtScript : MonoBehaviour
{
    public Text text;
    public RectTransform rectTransform;

    public float speedModifier = 2;
    public float minSize = 200;
    public float maxSize = 300;
    public string[] possibleLetters = new[]{"A", "E"};

    private float yVel = 0;

    public void Initialize()
    {
        yVel = 1.5f;
        text.text = possibleLetters[UnityEngine.Random.Range(0, possibleLetters.Length)];
        text.fontSize = (int)UnityEngine.Random.Range(minSize, maxSize);
        rectTransform.anchoredPosition = new Vector3(UnityEngine.Random.Range(0, Screen.width) - Screen.width/2, Screen.height, 0f);;
    }

    // Update is called once per frame
    void Update()
    {
        yVel += 9.8f* Time.deltaTime * speedModifier;

        var pos = rectTransform.anchoredPosition;
        pos.y -= yVel;
        rectTransform.anchoredPosition = pos;
    }
}
