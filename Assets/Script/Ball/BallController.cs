using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float verticalMaxSpeed;
    [SerializeField] private float horizontalMaxSpeed;

    private Rigidbody2D ballRigidBody;

    private string ballGroundTag;

    public event Action<string> OnPlayerScore;

    private bool isPlayerScored;

    private bool isOnServe;

    [SerializeField] private LayerMask groundLayer;


    public void SetBallOnServe(bool value)
    {
        isOnServe = value;
    }

    public void SetBallGroundTag(string tag)
    {
        ballGroundTag = tag;
    }

    public bool IsBallOnServe()
    {

         return isOnServe;
    }
    public string GetBallGroundTag()
    {
        return ballGroundTag;
    }

    void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        isOnServe = true;
    }

    void Update()
    {
        if(ballRigidBody.velocity.x > verticalMaxSpeed)
        {
            ballRigidBody.velocity = new Vector2(verticalMaxSpeed, ballRigidBody.velocity.y);
        }

        if (ballRigidBody.velocity.y > horizontalMaxSpeed)
        {
            ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, horizontalMaxSpeed);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,10, groundLayer);
        if (hit)
        {
            if (hit.transform.tag != ballGroundTag)
            {
                ballGroundTag = hit.transform.tag;
            }
        }

        transform.right = ballRigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Floor1") || other.gameObject.CompareTag("Floor2")) && !IsBallOnServe())
        {
            ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x,-1);
            PhysicEnabled(false);
            SetBallOnServe(true);
            if (OnPlayerScore != null)
            {
                OnPlayerScore(other.gameObject.tag);
            }        
        }
    }

    public void PhysicEnabled(bool value)
    {
       ballRigidBody.simulated = value;    
    }

    public void MoveBall(float direction,float verticalSwingSpeed, float horizontalSwingSpeed, float swingPosition)
    {
        ballRigidBody.velocity = new Vector2(direction * verticalSwingSpeed, (swingPosition + horizontalSwingSpeed));       
        AudioManager.Instance.PlayRandomSwingSound();
    }
}
