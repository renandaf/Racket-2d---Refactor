using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RacketController : Racket
{
    [SerializeField] private Character character;
    [SerializeField] private BallController ball;
    [SerializeField] private float swingAnimationSpeed;
    [SerializeField] private float backAnimationSpeed;
    [SerializeField] private BoxCollider2D racketCollider;
    [SerializeField] private HingeJoint2D joint;

    private float xSwingSpeed;
    private float ySwingSpeed;

    private JointMotor2D motor;
    private bool swingDelay;
    private bool ballDelay;

    private void Awake()
    {
        motor = joint.motor;
    }   

    private void Update()
    {
        if (Input.GetKeyDown(character.GetControl("swing")) && !swingDelay)
        {
            Swing();
            if (ball.IsBallOnServe() && IsRacketOnServe()) {
                GameManager.Instance.FinishServe();
                BallSwing();               
            }           
        }

        if (Input.GetKeyDown(character.GetControl("smash")) && !swingDelay && !IsRacketOnServe())
        {
            Smash();
        }
    }

    public void Smash()
    {
        swingDelay = true;
        racketCollider.enabled = true;
        xSwingSpeed = UnityEngine.Random.Range(11f, 15f);
        ySwingSpeed = UnityEngine.Random.Range(-1f, 2f);
        motor.motorSpeed = swingAnimationSpeed;
        joint.motor = motor;
        Invoke("SwingBack", 0.3f);
        Invoke("SwingDelay", 0.7f);
    }

    public void Swing()
    {
        swingDelay = true;
        racketCollider.enabled = true;
        xSwingSpeed = UnityEngine.Random.Range(6f,9f);
        ySwingSpeed = 6f;
        motor.motorSpeed = swingAnimationSpeed;
        joint.motor = motor;
        Invoke("SwingBack", 0.3f);
        Invoke("SwingDelay", 0.7f);
    }

    private void SwingBack()
    { 
        motor.motorSpeed = backAnimationSpeed;
        joint.motor = motor;
        racketCollider.enabled = false;
    }

    private void SwingDelay()
    {
        swingDelay = false;
    }

    private void BallDelay()
    {
        ballDelay = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball") && !ballDelay)
        {
            BallSwing();
        }       
    }
    private void BallSwing()
    {
        ball.MoveBall(character.GetDirection().right.x, xSwingSpeed, ySwingSpeed, transform.position.y);
        ballDelay = true;
        Invoke("BallDelay", 0.5f);
    }
}
