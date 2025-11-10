using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AIController : MonoBehaviour
{
    [SerializeField] private Character characterData;
    [SerializeField] private BallController ballController;

    private Rigidbody2D rb;
    private float horizontal;
    private float target;
    private float offsetPosition;
    private bool ballCheck;
    private bool jumpCheck;

    public bool GetJumpCheck()
    {
        return jumpCheck;
    }

    public void SetJumpCheck(bool value)
    {
        jumpCheck = value;
    }

    public void SetBallCheck(bool value)
    {
        ballCheck = value;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        
    }
    private void Start()
    {
        target = characterData.GetDefaultStand();
    }

    private void Update()
    {
        if (!ballController.IsBallOnServe())
        {
            if (characterData.GetTag() != ballController.GetBallGroundTag())
            {
                if (!ballCheck)
                {
                    target = ballController.transform.position.x;
                }
                else
                {
                    if (characterData.IsGrounded())
                    {
                        target = characterData.GetDefaultStand();
                    }
                }
            }
            else
            { 
                target = characterData.GetDefaultStand();
                if (ballCheck)
                {
                    jumpCheck = Random.Range(0, 2) == 0;
                    ballCheck = false;                    
                }
            }
            offsetPosition = transform.position.x - (characterData.GetDirection().right.x * 0.7f);


            if (Mathf.Abs(target - offsetPosition) <= 0.2)
            {
                horizontal = 0;
            }
            else
            {
                if (offsetPosition >= target)
                {
                    horizontal = -1;
                }

                if (offsetPosition <= target)
                {
                    horizontal = 1;
                }
            }
            if (jumpCheck)
            {
                if (Vector2.Distance(new Vector2(offsetPosition, transform.position.y), ballController.transform.position) <= characterData.GetJumpHeight())
                {
                    AIJump();
                }
            }
        }          
    }

    private void FixedUpdate()
    {      
        rb.velocity = new Vector2(horizontal * characterData.GetSpeed(), rb.velocity.y);
    }

    public void AIJump()
    {
        if (characterData.IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, Vector2.Distance(new Vector2(offsetPosition,transform.position.y), ballController.transform.position) + characterData.GetJumpHeight());
        }
    }
}
