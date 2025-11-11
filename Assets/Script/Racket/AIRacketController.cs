using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AIRacketController : Racket
{
    [SerializeField] private Character character;
    [SerializeField] private BallController ball;
    [SerializeField] private float swingAnimationSpeed;
    [SerializeField] private float backAnimationSpeed;
    [SerializeField] private BoxCollider2D racketCollider;
    [SerializeField] private HingeJoint2D joint;
    [SerializeField] private AIController controller;

    private float xSwingSpeed;
    private float ySwingSpeed;

    private JointMotor2D motor;
    private bool swingDelay;
    private bool ballDelay;

    public bool GetSwingDelay()
    {
        return swingDelay;
    }

    private void Awake()
    {
        motor = joint.motor;
    }
    private void Update()
    {
        if (IsRacketOnServe())
        {
            controller.SetBallCheck(true);
            controller.SetJumpCheck(false);
            Swing();
            BallSwing();
            GameManager.Instance.FinishServe();
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
        xSwingSpeed = UnityEngine.Random.Range(7f, 10f);
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
