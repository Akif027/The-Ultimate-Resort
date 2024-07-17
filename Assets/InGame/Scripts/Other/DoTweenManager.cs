using UnityEngine;
using DG.Tweening;

public static class DoTweenManager
{
    // Method to move an object to a target position
    public static Tween MoveTo(Transform target, Vector3 position, float duration, Ease easeType = Ease.Linear)
    {
        return target.DOLocalMove(position, duration).SetEase(easeType);
    }

    // Method to scale an object
    public static Tween ScaleTo(Transform target, Vector3 scale, float duration, Ease easeType = Ease.Linear)
    {
        return target.DOScale(scale, duration).SetEase(easeType);
    }

    // Method to rotate an object
    public static Tween RotateTo(Transform target, Vector3 rotation, float duration, Ease easeType = Ease.Linear)
    {
        return target.DORotate(rotation, duration).SetEase(easeType);
    }

    // Method to fade a CanvasGroup
    public static Tween FadeTo(CanvasGroup canvasGroup, float endValue, float duration, Ease easeType = Ease.Linear)
    {
        return canvasGroup.DOFade(endValue, duration).SetEase(easeType);
    }

    // Method to shake an object
    public static Tween Shake(Transform target, float duration, float strength, int vibrato = 10, float randomness = 90, bool fadeOut = true)
    {
        return target.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }

    public static void PlayUpgradeAnimation(Transform target, float scaleUpDuration, float scaleDownDuration, float scaleMultiplier, Color upgradeColor, GameObject upgradeEffect = null, Transform Effectpos = null)
    {        // Instantiate upgrade effect if available
        if (upgradeEffect != null)
        {
            //   Vector3 newPosition = new Vector3(target.position.x + 2, target.position.y + 6, target.position.z);
            Object.Instantiate(upgradeEffect, Effectpos.position, Quaternion.identity);
        }
        Vector3 originalScale = target.localScale;
        Renderer objectRenderer = target.GetComponentInChildren<Renderer>();
        Color originalColor = objectRenderer != null ? objectRenderer.material.color : Color.white;

        // Scale up and down animation
        ScaleTo(target, originalScale * scaleMultiplier, scaleUpDuration, Ease.OutQuad)
            .OnComplete(() => ScaleTo(target, originalScale, scaleDownDuration, Ease.InQuad));



        // Change color if the object has a renderer
        if (objectRenderer != null)
        {
            objectRenderer.material.DOColor(upgradeColor, scaleUpDuration).OnComplete(() =>
            {
                objectRenderer.material.DOColor(originalColor, scaleDownDuration);
            });
        }


    }
}
