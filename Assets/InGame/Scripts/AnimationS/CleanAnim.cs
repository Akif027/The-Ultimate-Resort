using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CleanAnim : GameManager
{
    Animator _Anim;
    [SerializeField] float cleaningTime;
    public string furnitureName;
    public UnityEvent OnCleaned;
    [SerializeField] Room room;
    private bool isCleaning = false;
    protected override void Initialize()
    {
        _Anim = GetComponent<Animator>();

    }

    public void PlayAnimation(String AnimName)
    {
        _Anim.SetTrigger(AnimName);
        Debug.Log("playing " + AnimName);
    }

    public void StartCleaning()
    {
        StartCoroutine(WaitAndClean());
    }

    IEnumerator WaitAndClean()
    {
        // Prevent multiple coroutines from starting
        if (isCleaning) yield break;

        isCleaning = true;

        // Wait for 5 seconds
        yield return new WaitForSeconds(cleaningTime);

        // Play the cleaning animation
        PlayAnimation("Clean");
        OnCleaned?.Invoke();
        RoomData roomData = RoomManager.instance.FindRoomData(room.RoomNumber);
        if (roomData != null)
            roomData.isClean = true;
        else
            Debug.LogError("Room not found in cleanAnim Script");


    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            StartCleaning();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            isCleaning = false; // Reset cleaning state
        }
    }
}
