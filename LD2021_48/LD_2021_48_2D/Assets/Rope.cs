using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public AnimationCurve letGoAnimation;
    public float letGoSpeed = 1;

    public Player player;
    public GameObject Graphic;
    public SpriteRenderer GraphicSpriteRenderer;
    public GameObject hookPrefab;

    public float ropeInitXMove = 0.65f;
    public float ropeInitYMove = 0.5f;

    public float relativeLength = 0.7f;
    public float stiffness = 0.5f;
    public float reelSpeed = 1f;

    private float wantedLength = -1;
    private GameObject hook;
    private float letGoTime = -1;

    public float graphicModifier = 1;
    public float graphicMoveModifier = 0;

    public float maxLength = 3.5f;

    private float GetRealLength()
    {
        var posDiff = hook.transform.position - transform.position;
        var length = Mathf.Sqrt(posDiff.x * posDiff.x + posDiff.y * posDiff.y);
        return length; 
    }

    private Vector3 GetNormalizedDir()
    {
        var posDiff = hook.transform.position - transform.position;
        var length = GetRealLength();   
        var normalizedDir = posDiff / length;
        return normalizedDir;
    }

    public Vector2 GetVelocity()
    {
        var length = GetRealLength(); 
        var normalizedDir = GetNormalizedDir();
        var wantedPoint = transform.position - (normalizedDir * (wantedLength - length));

        var resultVel = (wantedPoint - transform.position) * stiffness * 0.036f;
        return new Vector2(resultVel.x, -resultVel.y);
    }

    public void UpdateRope()
    {
        if (wantedLength > maxLength)
        {
            player.ToggleRope(false);
            return;
        }

         if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            wantedLength -= reelSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            wantedLength += reelSpeed * Time.deltaTime;
        }
    }

    public void UpdateGraphics()
    {
        if (player.IsRoped())
        {
            letGoTime = -1;

            var length = GetRealLength();
            GraphicSpriteRenderer.size = new Vector2(GraphicSpriteRenderer.size.x, length * graphicModifier);

            var normalizedDir = GetNormalizedDir();
            float angle = Mathf.Atan2(normalizedDir.x, normalizedDir.y) * Mathf.Rad2Deg;
            Graphic.transform.rotation =  Quaternion.Euler(0, 0, angle*-1);

            var moveLength = length + graphicMoveModifier;
            var playerScale = player.transform.localScale.y;

            var position = new Vector3(player.transform.position.x + normalizedDir.x * moveLength * playerScale, 
            player.transform.position.y +  normalizedDir.y * moveLength * playerScale, 1);
            Graphic.transform.localPosition = position;
        }
        else 
        {
            if (letGoTime == -1)
            {
                letGoTime = Time.time;
            }

            var size = GraphicSpriteRenderer.size;
            size.y *= letGoAnimation.Evaluate(((Time.time-letGoTime)*letGoSpeed));
            GraphicSpriteRenderer.size = size;
        }
    }

    public void End()
    {
        Destroy(hook);
    }

    public void Create(World world)
    {
        bool foundCollision = false;
        
        float posX = transform.position.x;
        float posY = transform.position.y;
        while(!foundCollision)
        {
            posX += ropeInitXMove * player.mirrored;
            posY += ropeInitYMove;

            var pixelPos = world.GetPixelPosition(new Vector3(posX, posY, 1));

            if (world.IsCollision(pixelPos) != CollsionType.none)
            {
                foundCollision = true;
                var worldPos = new Vector3(posX, posY, 1);
                GameObject taste = Instantiate(hookPrefab);
                taste.transform.position = worldPos;
                hook = taste;

                UpdateWantedLength();

                var xDiff = hook.transform.position.x - transform.position.x;
                var yDiff = hook.transform.position.y - transform.position.y;
                wantedLength = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * relativeLength;
            }
        }
    }

    public void UpdateWantedLength()
    {
        var xDiff = hook.transform.position.x - transform.position.x;
        var yDiff = hook.transform.position.y - transform.position.y;
        wantedLength = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * relativeLength;
    } 

    void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;

        //var goal = transform.position;
        //goal.x += ropeInitXMove * 100; 
        //goal.y += ropeInitYMove * 100; 

        //Gizmos.DrawLine(transform.position, goal);

         Gizmos.color = Color.red;

        if (hook != null)
        {
            Gizmos.DrawSphere(hook.transform.position, 0.2f);

            Gizmos.color = Color.green;

            var posDiff = hook.transform.position - transform.position;
            var length = Mathf.Sqrt(posDiff.x * posDiff.x + posDiff.y * posDiff.y); 
            
            var normalizedDir = posDiff / length;
            var wantedPoint = transform.position - (normalizedDir * (wantedLength - length));
            Gizmos.DrawSphere(wantedPoint, 0.2f);

            var vel = GetVelocity();
            var goal2 = transform.position;
            goal2.x += vel.x * 50; 
            goal2.y += vel.y * 50; 
            Gizmos.DrawLine(transform.position, goal2);

             //Debug.Log("wantedPoint " + wantedPoint);
        //Debug.Log("transform.position " + transform.position);
        //Debug.Log("hook " + hook);
        //Debug.Log("vel " + vel);
        }
    }
}
