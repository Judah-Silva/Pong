using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public int paddle = 1;
    public float unitsPerSecond = .3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float verticalValue;
        if (paddle == 1)
        {
            verticalValue = Input.GetAxis("Paddle 1");
        }
        else
        {
            verticalValue = Input.GetAxis("Paddle 2");
        }
        
        Vector3 force = Vector3.up * (unitsPerSecond * verticalValue);
        // Debug.Log($"Force on key press: {force}");

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // rigidbody.AddForce(force, ForceMode.Acceleration);

        float yPos = rigidbody.transform.position.y;
        if ((yPos >= 6.05 && verticalValue < 0) || (yPos <= -6.7 && verticalValue > 0) || (yPos < 6.05 && yPos > -6.7))
        {
            rigidbody.transform.position += Vector3.up * (unitsPerSecond * verticalValue);
        }
    }
}
