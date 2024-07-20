using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MoneyEffect : MonoBehaviour
{
    public GameObject moneyPrefab;
    public Vector3 offset = Vector3.up;
    public float timeGenRate = 0.1f;
    public float timeMove = 0.3f;
    public AnimationCurve acYUp;
    public AnimationCurve acYDown;
    public UnityEvent onClick;

    private Vector3 scaleFirst;

    private void Start()
    {
        scaleFirst = transform.localScale;
    }

    private IEnumerator Run(HotelManager character)
    {
        Transform target = character.transform;

        while (true)
        {
            if (target != null && Game.Instance.gameData.money.Round() > 0)
            {
                GameObject g = Instantiate(moneyPrefab);
                g.transform.position = target.position + offset;

                AnimationCurve animationCurve = target.position.y < transform.position.y ? acYUp : acYDown;

                g.transform.DOMoveX(transform.position.x, timeMove).SetAutoKill(false);
                g.transform.DOMoveY(transform.position.y, timeMove).SetEase(animationCurve).SetAutoKill(false);
                g.transform.DOMoveZ(transform.position.z, timeMove).SetAutoKill(false);

                Destroy(g, timeMove);

                g.transform.eulerAngles = new Vector3(Random.RandomRange(0, 360), Random.RandomRange(0, 360), Random.RandomRange(0, 360));
                g.transform.DOLocalRotate(Vector3.zero, timeMove).SetAutoKill(false);

                yield return new WaitForSeconds(timeGenRate);
                transform.DOScale(scaleFirst * 1.2f, 0.1f).onComplete += () =>
                {
                    transform.localScale = scaleFirst;
                };

                onClick.Invoke();

            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HotelManager character = other.GetComponent<HotelManager>();
        if (character != null)
        {
            StartCoroutine(Run(character));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopSpend();
    }

    public void StopSpend()
    {
        StopAllCoroutines();

    }
}
