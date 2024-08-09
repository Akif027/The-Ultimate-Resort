using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyPlace : MonoBehaviour
{
    public SortSlot sortSlot;

    public float timeRate = 0.1f;
    protected float timeCount;
    protected SecuredDouble profit;
    public GameObject moneyPrefab;
    public int count = 6;

    [SerializeField] SecuredDouble ResortCharges = 10;


    void Start()
    {

        ResortCharges = Game.Instance.gameData.GetResortFee();

    }
    public void Add(SortObject so)
    {
        sortSlot.AddObject(so);
    }


    public void PlaceMoney()
    {

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(moneyPrefab);

            SortObject sortObject = g.GetComponent<SortObject>();
            sortSlot.AddObjectNotEffect(sortObject);
            SoundManager.Instance.PlayGetObject(transform.position);
        }
    }
    public void Put(Transform character)
    {
        SortObject sortObject = sortSlot.EndObject;

        if (sortObject != null)
        {
            sortSlot.RemoveObject(sortObject);
            sortObject.MoveToWorldPosition(character);
            sortObject.transform.DOScale(Vector3.zero, 0.6f);
            Destroy(sortObject.gameObject, 0.3f);
            MoneyValue moneyValue = sortObject.GetComponent<MoneyValue>();
            moneyValue.value = ResortCharges.Round();
            if (moneyValue != null)
            {
                Game.Instance.gameData.AddMoney(moneyValue.value);

                GameObject text = Instantiate(Game.Instance.gameData.textEffect);
                text.transform.position = character.transform.position + Vector3.up;
                text.GetComponent<TextEffect>().Initialize("+" + moneyValue.value.Round().ToString());

                SoundManager.Instance.PlayGetMoneySound(transform.position);
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        timeCount += Time.deltaTime;
        if (timeCount >= timeRate)
        {
            HotelManager character = other.GetComponent<HotelManager>();
            if (character != null)
            {
                Put(character.sortSlot.transform);
            }

            timeCount = 0;
        }
    }
}
