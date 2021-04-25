using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollsionType
{
  none,
  ground,
  lava,
  bounce
}

public class World : MonoBehaviour
{
  public int pixelWidth = 1024;
  public int pixelHeight = 8000;

  public Transform leftTop;
  public Transform rightTop;
  public Transform leftBottom;
  public Transform rightBottom;

  public Texture2D worldCollision;

    private float transformX = 0;
    private float transformY = 0;
    
    void Start()
    {

    }

    public Vector2 GetPixelPosition(Vector3 position)
    {
        var totalX = rightTop.position.x - leftTop.position.x;
        var totalY = leftTop.position.y - leftBottom.position.y;

        var transformedY = (position.y / totalY * pixelHeight) + pixelHeight*0.5f;
        var transformedX = (position.x / totalX * pixelWidth) + pixelWidth*0.5f;

        return new Vector2(transformedX, transformedY);
    }

    public Vector3 GetWorldPosition(Vector2 position)
    {
        var totalX = rightTop.position.x - leftTop.position.x;
        var totalY = leftTop.position.y - leftBottom.position.y;

        var transformedY = (position.y / pixelHeight * totalY) - pixelHeight*0.5f;
        var transformedX = (position.x / pixelWidth * totalX) - pixelWidth*0.5f;

        return new Vector3(transformedX, transformedY, 1);
    }

    public CollsionType IsCollision(Vector2 position)
    {
       Color32 color = worldCollision.GetPixel((int)position.x, (int)position.y);
        if (color.r > 200 && color.b > 200)
          return CollsionType.ground;
        if (color.r > 200)
          return CollsionType.lava;
        if (color.b > 200)
          return CollsionType.bounce;
        return CollsionType.none;
    }

    void Update()
    {
        
    }
}
