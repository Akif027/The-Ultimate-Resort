using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{





    protected virtual void Initialize()
    {

    }

    // Common update logic that can be overridden by subclasses
    protected virtual void UpdateGame()
    {
    }

    // Unity's Start method, which calls Initialize
    private void Start()
    {
        Initialize();
    }

    // Unity's Update method, which calls UpdateGame
    private void Update()
    {
        UpdateGame();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Common logic for handling collision events
    }

    // Unity's OnTriggerEnter method, which can be overridden by subclasses
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Common logic for handling trigger events
    }
}