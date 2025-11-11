using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode jump;
    [SerializeField] private KeyCode swing;
    [SerializeField] private KeyCode smash;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Racket playerRacket;
    [SerializeField] private Transform direction;
    [SerializeField] private float defaultStand;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string groundTag;

    public float GetDefaultStand()
    {
        return defaultStand; 
    }

    public Transform GetDirection()
    {
        return direction;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public string GetTag()
    {
        return groundTag;
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }

    public Racket GetPlayerRacket() { 
        return playerRacket;
    }

    public KeyCode GetControl(string controlName)
    {
        switch (controlName)
        {      
            case "left":
                return left;
            case "right":
                return right;
            case "jump":
                return jump;
            case "swing":
                return swing;
            case "smash":
                return smash;
            default:
                return KeyCode.None;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
