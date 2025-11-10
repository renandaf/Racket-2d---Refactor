using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character player1;
    [SerializeField] private Character player2;
    [SerializeField] private Character AI;

    [SerializeField] private BallController ball;


    [SerializeField] private Text p1ScoreUI;
    [SerializeField] private Text p2ScoreUI;
    [SerializeField] private Text winnerUI;
    [SerializeField] private Text timerUI;
    [SerializeField] private Button startAI;
    [SerializeField] private Button startPlayer;
    [SerializeField] private Button restart;
    [SerializeField] private Button exit;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject endUI;
    [SerializeField] private Light2D spotlight;
    [SerializeField] private BoxCollider2D wall1;
    [SerializeField] private BoxCollider2D wall2;

    private float p1score;
    private float p2score;

    private int timerValue;
    private Character servePlayer;

    private RacketController racketServe;

    private float currentTime;
    [SerializeField] private float time;
    private bool paused;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        paused = true;
        startAI.onClick.AddListener(GameStartAI);
        startPlayer.onClick.AddListener(GameStartPlayer);
        exit.onClick.AddListener(GameExit);
    }


    private void FixedUpdate()
    {
        if (!paused)
        {
            currentTime -= Time.fixedDeltaTime;
            timerValue = (int)currentTime;
            timerUI.text = timerValue.ToString();

            if (currentTime <= 0)
            {
                ball.gameObject.SetActive(false);
                Whistle();
                Invoke("Whistle", 1);
                Invoke("GameEnd", 3);
                paused = true;
            }
        }   
    }

    private void Whistle()
    {
        AudioManager.Instance.PlayWhistleSound();
    }

    private void GameStartAI()
    {
        AudioManager.Instance.StopTheme();
        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(GameStartAI);

        ball.OnPlayerScore += AddScore;

        ResetScore();
        currentTime = time;
        timerUI.text = currentTime.ToString();

        player1.transform.position = new Vector2(player1.GetDefaultStand(), 0.5f);
        AI.transform.position = new Vector2(AI.GetDefaultStand(), 0.5f);

        startUI.SetActive(false);
        endUI.SetActive(false);

        player1.gameObject.SetActive(true);
        AI.gameObject.SetActive(true);

        ball.gameObject.SetActive(true);
        spotlight.enabled = true;

        gameUI.SetActive(true);

        if (Random.value < 0.5f)
        {
            PlayerServe(player1);
        }
        else
        {
            PlayerServe(player1);
        }
    }

    private void GameStartPlayer()
    {
        AudioManager.Instance.StopTheme();

        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(GameStartPlayer);

        ball.OnPlayerScore += AddScore;

        ResetScore();

        currentTime = time;
        timerUI.text = currentTime.ToString();

        player1.transform.position = new Vector2(player1.GetDefaultStand(), 0.5f);
        player2.transform.position = new Vector2(player2.GetDefaultStand(), 0.5f);

        startUI.SetActive(false);
        endUI.SetActive(false);

        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        ball.gameObject.SetActive(true);

        spotlight.enabled = true;

        gameUI.SetActive(true);

        if (Random.value < 0.5f)
        {
            PlayerServe(player1);
        }
        else
        {
            PlayerServe(player2);
        }
    }

    private void AddScore(string tag)
    {
        if (!paused)
        {
            AudioManager.Instance.PlayWhistleSound();
            if (tag == player1.GetTag())
            {
                p1score += 1;
                p1ScoreUI.text = p1score.ToString();              
            }
            else if (tag == player2.GetTag())
            {
                p2score += 1;
                p2ScoreUI.text = p2score.ToString();                
            }
            StartCoroutine(ServeStart(tag));
        }
    }

    private IEnumerator ServeStart(string tag)
    {
        yield return new WaitForSeconds(2f);
        if (tag == player1.GetTag())
        {
            PlayerServe(player1);
            ball.SetBallGroundTag(AI.GetTag());
        }
        else if (tag == player2.GetTag())
        {
            PlayerServe(AI);
            ball.SetBallGroundTag(player1.GetTag());
        }                
    }

    private void ResetScore()
    {
        p1score=0; p2score=0;
        p1ScoreUI.text = p1score.ToString();
        p2ScoreUI.text = p2score.ToString();
    }

    private void PlayerServe(Character player)
    {
        WallEnabled(false);
        ball.transform.parent = player.GetPlayerRacket().transform;
        ball.transform.localPosition = player.GetPlayerRacket().GetServePosition();
        ball.SetBallOnServe(true);
        ball.PhysicEnabled(false);
        player.GetPlayerRacket().SetRacketOnServe(true);
        paused = true;
        player1.transform.position = new Vector2(player1.GetDefaultStand(), 0.5f);
        AI.transform.position = new Vector2(AI.GetDefaultStand(), 0.5f);
    }

    public void FinishServe()
    {
        ball.SetBallOnServe(false);
        player1.GetPlayerRacket().SetRacketOnServe(false);
        AI.GetPlayerRacket().SetRacketOnServe(false);
        WallEnabled(true);
        ball.transform.parent = null;
        ball.PhysicEnabled(true);
        paused = false;
    }

    private void GameEnd()
    {
        StopCoroutine("ServeStart");
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        spotlight.enabled = false;
        gameUI.SetActive(false);

        if (p1score > p2score)
        {
            winnerUI.text = "Player 1 Win";
        }
        else if (p2score > p1score)
        {
            winnerUI.text = "Player 2 Win";
        }
        else
        {
            winnerUI.text = "Draw";
        }
        endUI.gameObject.SetActive(true);
    }

    private void GameExit()
    {
        AudioManager.Instance.PLayTheme();
        startUI.gameObject.SetActive(true);
        endUI.gameObject.SetActive(false);
        ball.OnPlayerScore -= AddScore;
    }

    private void WallEnabled(bool value)
    {
        if (value) {
            wall1.excludeLayers = LayerMask.GetMask();
            wall2.excludeLayers = LayerMask.GetMask();
        }
        else
        {
            wall1.excludeLayers = LayerMask.GetMask("Ball");
            wall2.excludeLayers = LayerMask.GetMask("Ball");
        }   
    }
}
