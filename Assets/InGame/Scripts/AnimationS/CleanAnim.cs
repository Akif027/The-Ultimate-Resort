using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CleanAnim : MonoBehaviour
{
    Animator _Anim;
    [SerializeField] float cleaningTime;
    public string furnitureName;
    public UnityEvent OnCleaning;
    [SerializeField] Room room;
    private bool isCleaning = false;

    [SerializeField] Transform ProgressSignPos;
    [SerializeField] RadialProgressBar circularProgressBar;
    void Start()
    {
        _Anim = GetComponent<Animator>();

    }

    public void PlayAnimation(string animName)
    {
        // Get the current state index of the base layer
        int currentStateIndex = _Anim.GetCurrentAnimatorStateInfo(0).shortNameHash;

        // Check if the current state matches the animation we want to play
        if (_Anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            //  Debug.Log("Animation is already playing.");
            return; // Do nothing if the animation is already playing
        }

        // Set the trigger for the animation
        _Anim.SetTrigger(animName);

        Debug.Log("Playing " + animName);
    }



    private void StartCleaning()
    {

        if (circularProgressBar.isActive) return;


        ExecuteCleaningProgressSignEffect();
        //  TimerManager.Instance.ScheduleAction(cleaningTime, OnCleaned);



    }
    [SerializeField] GameObject energizedEffect = null;
    public void PlaceCleaningSign()
    {
        energizedEffect = ObjectPool.Instance.GetPooledObject("CleaningProgress");
        circularProgressBar = energizedEffect.GetComponent<RadialProgressBar>();
        energizedEffect.transform.SetPositionAndRotation(ProgressSignPos.position, ProgressSignPos.rotation);

    }
    private void ExecuteCleaningProgressSignEffect()
    {

        circularProgressBar.ActivateCountDown(cleaningTime);
    }
    private void OnCleaned()
    {
        PlayAnimation("Clean");
        RoomData roomData = RoomManager.instance.FindRoomData(room.RoomNumber);
        if (roomData != null)
        {
            roomData.isClean = true;
        }
        else
        {
            Debug.LogError("Room not found in cleanAnim Script");
        }

        // Optionally, reset the cleaning flag after performing the cleaning action
        isCleaning = false;

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {
            StartCleaning();
            circularProgressBar?.ResumeCountdown();
            circularProgressBar.OnComplete.AddListener(OnCleaned);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.gameObject.CompareTag("Player"))
        {

            circularProgressBar?.PauseCountdown();

        }
    }
}
