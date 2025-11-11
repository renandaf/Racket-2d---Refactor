using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    [SerializeField] private Transform servePosition;
    private bool isOnServe;

    public Vector2 GetServePosition()
    {
        return servePosition.localPosition;
    }

    public bool GetRacketOnServe()
    {
        return isOnServe;
    }

    public void SetRacketOnServe(bool value)
    {
        isOnServe = value;

    }
    public bool IsRacketOnServe()
    {
        return isOnServe;
    }
}
