using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UpgradeIt : MonoBehaviour
{

    [SerializeField] UpgradeData upgradeData;
    private Status status;
    public float spendTime = 10;
    public SecuredDouble SpendValue { get; set; }
    public SecuredDouble Cost { get; set; }

    public UnityEvent OnUpgrade;




    void Start()
    {
        status = GetComponentInParent<Status>();
        upgradeData = status.getUpgradedata();
        Cost = upgradeData.cost;

        // if (upgradeData.isUpgraded)
        // {
        //     gameObject.SetActive(false);
        // }
        // else
        // {
        //     gameObject.SetActive(true);
        // }



    }
    public void initilizeupgradeData(UpgradeData data)
    {
        upgradeData = data;
    }

    public virtual void SpendMoney()
    {
        if (Game.Instance.gameData.TrySpendMoney(Cost))
        {
            Cost -= Cost;
        }

        if (Cost <= 0)
        {
            TimerManager.Instance.ScheduleAction(spendTime, Upgrade);
        }
    }

    public virtual void Upgrade()
    {
        MoneyEffect moneyEffect = GetComponentInChildren<MoneyEffect>();
        if (moneyEffect != null)
        {
            moneyEffect.StopSpend();
        }

        Debug.Log("Upgraded");
        upgradeData.isUpgraded = true;
        OnUpgrade?.Invoke();


    }


}
