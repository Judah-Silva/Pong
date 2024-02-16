using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public Text score1;
    public Text score2;
    public GameObject leftPaddle;
    public GameObject rightPaddle;
    private int hitCounter = 0;
    private float ballSpeed = 10;
    private float speedModifier = 1.75f;
    private Rigidbody rb;
    private int player1Score = 0;
    private int player2Score = 0;
    private int scored = 0;
    private int lastHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartRound();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, ballSpeed + (speedModifier * hitCounter));
    }

    private void StartRound()
    {
        if (scored == 1)
        {
            rb.velocity = new Vector3(1, 0, 0) * (ballSpeed + (speedModifier * hitCounter));
        }
        else
        {
            rb.velocity = new Vector3(-1, 0, 0) * (ballSpeed + (speedModifier * hitCounter));
            
        }
    }

    private void ResetBall()
    {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, 0);
        hitCounter = 0;
        Invoke(nameof(StartRound), 2f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            hitCounter++;
            
            BoxCollider boxCollider = other.gameObject.GetComponent<BoxCollider>();
            Bounds bounds = boxCollider.bounds;
            float maxY = bounds.max.y;
            float minY = bounds.min.y;
            float otherY = transform.position.y;
            float percentAlong = (otherY - minY) / (maxY - minY);
            
            int paddle = 1;
            lastHit = 0;
            if (other.gameObject.name == "Paddle 2")
            {
                paddle = -1;
                lastHit = 1;
            }
            Quaternion rotation = Quaternion.Euler(0f, 0f, paddle * (-60f + (120f * percentAlong)));
            Vector3 bounceDirection = rotation * new Vector3(paddle, 0, 0);
            
            rb.velocity = bounceDirection * (ballSpeed + (speedModifier * hitCounter));

            AudioSource audioSource = GetComponent<AudioSource>();
            Debug.Log(speedModifier * hitCounter);
            audioSource.pitch = 1 + (speedModifier * hitCounter) / 15f;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Left Score")
        {
            player2Score++;
            scored = 2;
            if (player2Score == 11)
            {
                ResetGame();
            }
            else
            {
                Debug.Log($"Player 2 scored! Game score: {player1Score} : {player2Score}.");
                Animator animator = score2.GetComponent<Animator>();
                Debug.Log(animator);
                score2.text = player2Score.ToString();
                animator.SetBool("Score Changed", true);
                Invoke("ResetAnimations", 0.1f);
            }
            ResetBall();
        } else if (other.gameObject.name == "Right Score")
        {
            player1Score++;
            scored = 1;
            if (player1Score == 11)
            {
                ResetGame();
            }
            else
            {
                Debug.Log($"Player 1 scored! Game score: {player1Score} : {player2Score}.");
                Animator animator = score1.GetComponent<Animator>();
                Debug.Log(animator);
                score1.text = player1Score.ToString();
                animator.SetBool("Score Changed", true);
                Invoke("ResetAnimations", 0.1f);
            }
            ResetBall();
        } else if (other.gameObject.CompareTag("Ball Slow Down"))
        {
            print("Hit Ball Slow Down Powerup");
            hitCounter = 0;
            other.gameObject.SetActive(false);
        } else if (other.gameObject.CompareTag("Paddle Size Increase"))
        {
            print("Hit Paddle Size Increase Powerup");
            if (lastHit == 0)
            {
                leftPaddle.transform.localScale = new Vector3(1, 1, 7);
            }
            else
            {
                rightPaddle.transform.localScale = new Vector3(1, 1, 7);
            }
            other.gameObject.SetActive(false);
        }
    }

    void ResetAnimations()
    {
        Animator animator = score1.GetComponent<Animator>();
        Animator animator2 = score2.GetComponent<Animator>();
        
        animator.SetBool("Score Changed", false);
        animator2.SetBool("Score Changed", false);
    }

    void ResetGame()
    {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, 0);
        hitCounter = 0;
        if (scored == 1)
        {
            Debug.Log($"Game Over, Player 1 Wins!");
        }
        else
        {
            Debug.Log($"Game Over, Player 2 Wins!");
        }
        score1.text = "0";
        score2.text = "0";
        scored = 0;
        player1Score = 0;
        player2Score = 0;
        leftPaddle.transform.localScale = new Vector3(1, 1, 5);
        rightPaddle.transform.localScale = new Vector3(1, 1, 5);
        Invoke(nameof(StartRound), 5f);
    }
}
