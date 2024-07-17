using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UpgradeIt : MonoBehaviour
{
    [SerializeField] UpgradeData.Type type;
    [SerializeField] UpgradeData upgradeData;
    public float spendTime = 10;
    public SecuredDouble SpendValue { get; set; }
    public SecuredDouble Cost { get; set; }

    public UnityEvent OnUpgrade;




    void Start()
    {
        upgradeData = UpgradeManager.Instance.UpgradeDataValues(type);
        Cost = upgradeData.cost;

        if (upgradeData.isUpgraded)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }



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
        OnUpgrade?.Invoke();
        upgradeData.isUpgraded = true;

    }


}
