using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CleanAnim : MonoBehaviour
{
    public string furnitureName;
    public UnityEvent OnCleanedEvent;
    public bool IsCleaningComplete { get; private set; }

    [SerializeField] private Room room;
    [SerializeField] private float cleaningTime;
    [SerializeField] private Transform progressSignPos;
    [SerializeField] private RadialProgressBar circularProgressBar;
    [SerializeField] private GameObject energizedEffect;
    [SerializeField] private bool isThisRoom;
    [SerializeField] private bool isThisActivity;

    [SerializeField] private GameObject pillow1;
    [SerializeField] private GameObject pillow2;

    public bool ShowSign = false;


    void Start()
    {
#if UNITY_EDITOR
        if (ShowSign) //for testing only 
        {
            PlaceCleaningSign();
            TimerManager.Instance.ScheduleAction(5, () => ObjectPool.Instance.ReturnObjectToPool(energizedEffect, "CleaningProgress"));
            ShowSign = false;
        }
#endif

    }
    public void PlayDirtyBed()
    {
        DoTweenManager.MoveTo(pillow1.transform, new Vector3(0.32f, 0.275f, -0.024f), 1f, Ease.Linear);
        DoTweenManager.MoveTo(pillow2.transform, new Vector3(-0.42f, 0.24f, 0.29f), 1f, Ease.Linear);
    }

    public void PlayClean()
    {
        DoTweenManager.MoveTo(pillow2.transform, new Vector3(-0.67f, 0.27f, 0.35f), 1f, Ease.Linear);
        DoTweenManager.MoveTo(pillow1.transform, new Vector3(-0.7427292f, 0.2375813f, -0.3049469f), 1f, Ease.Linear);
    }

    private void StartCleaning()
    {
        if (circularProgressBar.isActive) return;

        ExecuteCleaningProgressSignEffect();
    }

    public void PlaceCleaningSign()
    {
        energizedEffect = ObjectPool.Instance.GetPooledObject("CleaningProgress");
        circularProgressBar = energizedEffect.GetComponent<RadialProgressBar>();
        energizedEffect.transform.SetPositionAndRotation(progressSignPos.position, progressSignPos.rotation);
    }

    private void ExecuteCleaningProgressSignEffect()
    {
        circularProgressBar.ActivateCountDown(cleaningTime);
    }

    private void OnCleanedForRoom()
    {
        PlayClean();
        RoomData roomData = RoomManager.instance.FindRoomData(room.RoomNumber);
        if (roomData != null)
        {
            roomData.isClean = true;
        }
        else
        {
            Debug.LogError("Room not found in CleanAnim script");
        }

        CompleteCleaning();
        OnCleanedEvent?.Invoke();
    }

    private void OnCleanedForActivity()
    {
        CompleteCleaning();
        OnCleanedEvent?.Invoke();
    }

    public void CompleteCleaning()
    {
        IsCleaningComplete = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartCleaning();
        circularProgressBar?.ResumeCountdown();
        circularProgressBar.OnComplete.AddListener(isThisRoom ? OnCleanedForRoom : OnCleanedForActivity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        circularProgressBar?.PauseCountdown();
    }
}
