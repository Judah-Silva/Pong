using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Paddle : MonoBehaviour
{
    public float unitsPerSecond = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float verticalValue = Input.GetAxis("Vertical");
        Vector3 force = Vector3.forward * verticalValue;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(force, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"We hit {other.gameObject.name}");

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Bounds bounds = boxCollider.bounds;
        float maxZ = bounds.max.z;
        float minZ = bounds.min.z;
        
        Debug.Log($"maxZ = {maxZ}, minZ = {minZ}");
        Debug.Log($"x pos of ball is {other.transform.position.x}");
        
        Quaternion rotation = Quaternion.Euler(60f, 0f, 0f);
        Vector3 bounceDirection = rotation * Vector3.up;
        
        Debug.Log($"bounceDirection: {bounceDirection}");
        
        Rigidbody rigidbody = other.rigidbody;
        rigidbody.AddForce(bounceDirection * 1000f, ForceMode.Force);
    }
}
