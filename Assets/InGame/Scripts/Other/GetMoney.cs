using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoney : MonoBehaviour
{
    public SortSlot sortSlot;

    public float time = 1f;
    protected float timeCount;

    public GameObject moneyPrefab;
    public int count = 6;
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
            TimerManager.Instance.ScheduleAction(time, () => DestroyAfterTime(sortObject));

        }
    }

    void DestroyAfterTime(SortObject sortObject)
    {

        sortSlot.RemoveObject(sortObject);

        Destroy(sortObject.gameObject);

    }

}
