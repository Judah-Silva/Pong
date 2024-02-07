using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;
    private int hitCounter = 0;
    private float ballSpeed = 10;
    private float speedModifier = 1.25f;
    private Rigidbody rb;
    private int player1Score = 0;
    private int player2Score = 0;
    private int scored = 0;

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
        transform.position = new Vector3(16.15f, 0, 5);
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
            
            int paddle = -1;
            if (other.gameObject.name == "Paddle 1")
            {
                paddle = 1;
            }
            Quaternion rotation = Quaternion.Euler(0f, 0f, paddle * (-60f + (120f * percentAlong)));
            Vector3 bounceDirection = rotation * new Vector3(paddle, 0, 0);
            
            rb.velocity = bounceDirection * (ballSpeed + (speedModifier * hitCounter));
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
                score2.text = player2Score.ToString();
            }
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
                score1.text = player1Score.ToString();
            }
        }
        ResetBall();
    }

    void ResetGame()
    {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(16.15f, 0, 5);
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
        Invoke(nameof(StartRound), 5f);
    }
}