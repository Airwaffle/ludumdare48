using System.Collections;
using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;

public class Player : MonoBehaviour
{
  public World world;
  public GameCamera camera;
  public Rope rope;

  public Transform feet;
  public Transform front;
  public Transform head;

  private bool grounded = false;
  private bool roped = false;
  private bool dead = false;
  private bool won = false;
  private AudioSource audio;

  public float scale = 0;
  public float xPos = 0;
  public float yPos = 0;
  public float mirrored = 1;

  public float xspeed = 0.3f;
  public float xVel = 0;
  public float yVel = 0;

  public float maxYVel = 0.2f;
  public float maxXVel = 0.02f;
  public float jumpYVel = -0.037f;
  public float gravity = 0.98f;

  public float xRopeSlowdown = 0.99f;
  public float yRopeSlowdown = 0.99f;
  
  public float notActionableForFirstSeconds = 0;

  public AudioClip[] deathClips; 
  public AudioClip bounceClip; 

  private float startTime;

  void Start()
  {
    xPos = transform.position.x;
    yPos = transform.position.y;
    scale = transform.localScale.x;
    audio = GetComponent<AudioSource>();
    startTime = Time.time;
  }

  public bool IsDead()
  {
    return dead;
  }

  public bool IsWon()
  {
    return won;
  }

  public bool IsRoped()
  {
    return roped;
  }

  void HandleRopeMovement()
  {
    var ropeMove = rope.GetVelocity();
    xVel += ropeMove.x; 
    yVel += ropeMove.y; 
    xVel *= xRopeSlowdown;
    yVel *= yRopeSlowdown;
  }

  void HandleXMovement()
  {
    if (Time.time - startTime < notActionableForFirstSeconds)
      return;

    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      xVel -= xspeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      xVel += xspeed * Time.deltaTime;
    }

    if (xVel > maxXVel)
        xVel = maxXVel;

    if (xVel < -maxXVel)
        xVel = -maxXVel;

    if (grounded)
      xVel *= 0.9f;

    if (xVel > 0)
    {
      mirrored = 1;
      transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
    } 
    else if (xVel < 0)
    {
      mirrored = -1;
      transform.localScale = new Vector3(transform.localScale.y * -1, transform.localScale.y, transform.localScale.z);
    }
    
    Vector2 frontPos = world.GetPixelPosition(front.position);
    CollsionType collision = world.IsCollision(frontPos);
    if (collision == CollsionType.ground)
      xVel = 0;
    else if (collision == CollsionType.lava)
      Die();
    else if (collision == CollsionType.bounce)
      xVel *= -2;
  }

  void HandleJumpInput()
  {
    if (Time.time - startTime < notActionableForFirstSeconds)
      return;
      
    if (grounded)
    {
      if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
      {
        Jump(jumpYVel);
      }
    }
    else 
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        ToggleRope(!roped);
      }
    }
  }

  void Update()
  {
    if (dead)
      return;

    HandleXMovement();
    HandleJumpInput();

    if (yVel < 0)
    {
      Vector2 headPos = world.GetPixelPosition(head.position);
      CollsionType headCollision = world.IsCollision(headPos);
      if (headCollision == CollsionType.ground)
        yVel = -yVel;
      else if (headCollision == CollsionType.lava)
        Die();
      else if (headCollision == CollsionType.bounce)
        yVel = -yVel * 2;
    }

    yVel += gravity * scale * Time.deltaTime;
    if (yVel > maxYVel)
      yVel = maxYVel;
      
    if (roped)
    {
      HandleRopeMovement();
      rope.UpdateRope();
    }

    if (!grounded)
      yPos -= yVel;
    xPos += xVel;

    Vector3 newPos = new Vector3(xPos, yPos, 1);
    transform.position = newPos;

    Vector2 screenPos = world.GetPixelPosition(feet.position);
    CollsionType collision = world.IsCollision(screenPos);

    if (roped)
      collision = CollsionType.none;

    if (collision == CollsionType.ground)
    {
      if (grounded != true)
      {
        camera.Shake();
      }
      grounded = true;
      ToggleRope(false);
    }
    else
    {
      grounded = false;
    }

    if (collision == CollsionType.lava)
    {
      Die();
    }
    else if (collision == CollsionType.bounce)
    {
      Bounce();
      audio.clip = bounceClip;
      audio.Play();
    }

    rope.UpdateGraphics();
  }

  void Bounce()
  {
      Jump(jumpYVel * 1.5f);
  }

  public void Win()
  {
    won = true;
  }

  void Jump(float vel)
  {
      yVel = vel;
      grounded = false;
  }

  void Die()
  {
    dead = true;
    var clip = UnityEngine.Random.Range(0, deathClips.Length);
    audio.clip = deathClips[clip];
    audio.Play();
  }

  public void ToggleRope(bool newRope)
  {
    if (newRope == roped)
      return;

    if (newRope)
      Debug.Log("StartRope");
    else 
      Debug.Log("EndRope");

    roped = newRope;
    rope.gameObject.SetActive(roped);
    if (roped)
      rope.Create(world);
    else 
      rope.End();
  }
}
