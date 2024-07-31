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

    Animation Playeranimation;
    void Start()
    {

        if (TargetPosPillow1 != null && TargetPosPillow2 != null)
        {
            initialPositionPillow1 = pillow1.transform.localPosition;
            initialPositionPillow2 = pillow2.transform.localPosition;
        }


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
        Vector3 effectPosition = transform.position + new Vector3(0, 2, 0);
        Effect.Instance.PlayEffect("CleanEffect", effectPosition);

    }

    private void StartCleaning()
    {
        if (circularProgressBar == null) return; // Ensure circularProgressBar is not null
        if (circularProgressBar.isActive) return;
        circularProgressBar?.OnComplete.RemoveAllListeners();
        circularProgressBar?.OnComplete.AddListener(isThisRoom ? OnCleanedForRoom : OnCleanedForActivity);
        ExecuteCleaningProgressSignEffect();
    }

    public void PlaceCleaningSign()
    {
        if (energizedEffect != null) return; // Check the flag


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

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCleaning();
        circularProgressBar?.ResumeCountdown();
    }

    private void OnTriggerStay(Collider other)
    {

        if (!other.CompareTag("Player")) return;
        Playeranimation = other.gameObject.GetComponent<Animation>();
        if (!IsCleaningComplete && isSignPlaced) Playeranimation?.ChangeState(AnimationState.Clean);

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Playeranimation.ChangeState(AnimationState.Idle);
        circularProgressBar?.PauseCountdown();
    }
}
