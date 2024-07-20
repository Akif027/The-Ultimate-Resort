using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextEffect : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
    public float height = 1;
    public float time = 1;
    public float delay = 0.5f;

    public void Initialize(string content)
    {
        text.text = content;
        transform.DOMove(transform.position + Vector3.up * height, time).SetEase(Ease.OutBack);
        text.DOFade(0, time).SetDelay(delay);
        spriteRenderer.DOFade(0, time).SetDelay(delay).onComplete += () =>
        {
            Destroy(gameObject);
        };
    }
}
