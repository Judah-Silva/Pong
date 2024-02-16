using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    public GameObject ballSlowDown;
    public GameObject paddleSizeIncrease;
    public Text player1ScoreText;
    public Text player2ScoreText;
    private GameObject ballSlowDownCopy;
    private GameObject paddleSizeIncreaseCopy;
    private bool firstPowerup = false;
    private bool secondPowerup = false;
    private bool scored = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ballSlowDown.SetActive(false);
        paddleSizeIncrease.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int player1Score = int.Parse(player1ScoreText.text);
        int player2Score = int.Parse(player2ScoreText.text);
        if (!firstPowerup && (player1Score == 5 || player2Score == 5))
        {
            ballSlowDownCopy = Instantiate(ballSlowDown, new Vector3(0f, -5f, 0f), new Quaternion());
            ballSlowDownCopy.SetActive(true);
            firstPowerup = true;
            scored = true;
        }

        if (!secondPowerup && ((player1Score > 8 && player2Score <= player1Score - 5) ||
                               (player2Score > 8 && player1Score <= player2Score - 5)))
        {
            paddleSizeIncreaseCopy = Instantiate(paddleSizeIncrease, new Vector3(0, 5, 0), new Quaternion());
            paddleSizeIncreaseCopy.SetActive(true);
            secondPowerup = true;
        }

        if (player1Score == 0 && player2Score == 0 && scored)
        {
            print("Game over :)");
            firstPowerup = false;
            secondPowerup = false;
            scored = false;
            Destroy(ballSlowDownCopy);
            Destroy(paddleSizeIncreaseCopy);
        }
    }
}
