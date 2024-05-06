using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : GameManager
{
    public Transform player;
    public Vector3 offset; // Offset from the player
    public float smoothSpeed = 0.125f; // Speed of the camera movement

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;


    }

}
