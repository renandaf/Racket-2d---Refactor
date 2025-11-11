using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSensor : MonoBehaviour
{
    [SerializeField] private AIRacketController racketController;
    [SerializeField] private AIController moveController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!racketController.GetSwingDelay())
        {
            if (moveController.GetJumpCheck())
            {
                if (!racketController.IsRacketOnServe())
                {
                    racketController.Smash();
                }
            }
            else
            {
                racketController.Swing();
            }
            moveController.SetBallCheck(true);
        }
    }
}
