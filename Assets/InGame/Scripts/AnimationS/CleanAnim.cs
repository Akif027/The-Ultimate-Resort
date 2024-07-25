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
    public bool IsCleaningComplete { get; set; }


    [SerializeField] private float cleaningTime;
    [SerializeField] private Transform progressSignPos;
    [SerializeField] private RadialProgressBar circularProgressBar;
    [SerializeField] private GameObject energizedEffect;
    [SerializeField] private bool isThisRoom;
    [SerializeField] private bool isThisActivity;

    [Header("RoomRelatedSetting ->")]
    [SerializeField] private GameObject pillow1;
    [SerializeField] private GameObject pillow2;
    [SerializeField] private Room room;
    [Header("Positions")]
    [SerializeField] private Transform TargetPosPillow1;
    [SerializeField] private Transform TargetPosPillow2;

    [Header("Durations")]
    [SerializeField] private float moveDuration = 1f;

    private Vector3 initialPositionPillow1;
    private Vector3 initialPositionPillow2;
    private bool isSignPlaced = false; // Add this flag
    public bool ShowSign = false;
    Animation Playeranimation;
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
        if (TargetPosPillow1 != null && TargetPosPillow2 != null)
        {
            initialPositionPillow1 = pillow1.transform.localPosition;
            initialPositionPillow2 = pillow2.transform.localPosition;
        }
        // Uncomment or modify these lines as needed
        // DoTweenManager.MoveTo(pillow1.transform, new Vector3(0.32f, 0.275f, -0.024f), 1f, Ease.Linear);
        // DoTweenManager.MoveTo(pillow2.transform, new Vector3(-0.42f, 0.24f, 0.29f), 1f, Ease.Linear);

    }

    public void PlayDirty()
    {

        DoTweenManager.MoveTo(pillow1.transform, TargetPosPillow1.localPosition, TargetPosPillow1.rotation, moveDuration, Ease.Linear);


        DoTweenManager.MoveTo(pillow2.transform, TargetPosPillow2.localPosition, moveDuration, Ease.Linear);




    }

    public void PlayClean()
    {
        // Move the pillows back to their initial positions
        DoTweenManager.MoveTo(pillow1.transform, initialPositionPillow1, moveDuration, Ease.Linear);
        DoTweenManager.MoveTo(pillow2.transform, initialPositionPillow2, moveDuration, Ease.Linear);
    }

    private void StartCleaning()
    {
        if (circularProgressBar.isActive) return;

        ExecuteCleaningProgressSignEffect();
    }
    void Update()
    {


    }
    public void PlaceCleaningSign()
    {
        if (isSignPlaced) return; // Check the flag


        energizedEffect = ObjectPool.Instance.GetPooledObject("CleaningProgress");
        circularProgressBar = energizedEffect.GetComponent<RadialProgressBar>();
        energizedEffect.transform.SetPositionAndRotation(progressSignPos.position, progressSignPos.rotation);

        isSignPlaced = true; // Set the flag
    }

    private void ExecuteCleaningProgressSignEffect()
    {
        circularProgressBar.ActivateCountDown(cleaningTime);
    }

    private void OnCleanedForRoom()
    {
        PlayClean();
        //   Debug.LogError("is it cleaned");
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
        isSignPlaced = false;
        energizedEffect = null;
        circularProgressBar = null;
        Playeranimation.ChangeState(AnimationState.Idle);
        // circularProgressBar.OnComplete.RemoveAllListeners();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Playeranimation = other.gameObject.GetComponent<Animation>();
        if (!IsCleaningComplete && isSignPlaced) Playeranimation.ChangeState(AnimationState.Clean);
        StartCleaning();
        circularProgressBar?.ResumeCountdown();
        circularProgressBar.OnComplete.AddListener(isThisRoom ? OnCleanedForRoom : OnCleanedForActivity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Playeranimation.ChangeState(AnimationState.Idle);
        circularProgressBar?.PauseCountdown();
    }
}
