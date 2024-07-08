using UnityEngine;
using DG.Tweening;

public static class DoTweenManager
{
    // Method to move an object to a target position
    public static void MoveTo(Transform target, Vector3 position, float duration, Ease easeType = Ease.Linear)
    {
        target.DOLocalMove(position, duration).SetEase(easeType);
    }

    // Method to scale an object
    public static void ScaleTo(Transform target, Vector3 scale, float duration, Ease easeType = Ease.Linear)
    {
        target.DOScale(scale, duration).SetEase(easeType);
    }

    // Method to rotate an object
    public static void RotateTo(Transform target, Vector3 rotation, float duration, Ease easeType = Ease.Linear)
    {
        target.DORotate(rotation, duration).SetEase(easeType);
    }

    // Method to fade a CanvasGroup
    public static void FadeTo(CanvasGroup canvasGroup, float endValue, float duration, Ease easeType = Ease.Linear)
    {
        canvasGroup.DOFade(endValue, duration).SetEase(easeType);
    }

    // Method to shake an object
    public static void Shake(Transform target, float duration, float strength, int vibrato = 10, float randomness = 90, bool fadeOut = true)
    {
        target.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }

    // Add more methods as needed
}
