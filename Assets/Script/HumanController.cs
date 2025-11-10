using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Character characterData;
    
    private float horizontal;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void Update()
    {
         horizontal = Input.GetAxisRaw("Horizontal");
         PlayerJump();
    }

    private void FixedUpdate()
    {     
        if (Input.GetKey(characterData.GetControl("left")) || Input.GetKey(characterData.GetControl("right")))
        {
            if (characterData.IsGrounded())
            {
                transform.position += new Vector3(horizontal * characterData.GetSpeed() * Time.deltaTime, 0);
            }
            else
            {
                transform.position += new Vector3(horizontal * (characterData.GetSpeed() - 1) * Time.deltaTime, 0);
            }           
        }
        rb.velocity = new Vector2(0,rb.velocity.y);
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(characterData.GetControl("jump")) && characterData.IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, characterData.GetJumpHeight());
        }

        if (Input.GetKeyUp(characterData.GetControl("jump")) && !characterData.IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

}
