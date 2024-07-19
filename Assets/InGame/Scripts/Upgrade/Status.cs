using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public enum SlotStatus { Locked, Unlocked }
    public SlotStatus status = SlotStatus.Locked;
    public UpgradeData.Type upgradeType;
    [SerializeField] UpgradeData upgradeData;
    public GameObject upgradeSlot; // Single upgrade slot
    public GameObject UpgradeGameObj;
    public UpgradeData Inizilizedata
    {

        set
        {
            upgradeData = value;

        }
    }


    public void Unlock()
    {
        status = SlotStatus.Unlocked;
        UpdateUpgradeSlot();
    }

    public UpgradeData.Type getUpgradedataType()
    {

        return upgradeType;
    }
    public UpgradeData getUpgradedata()
    {

        return upgradeData;
    }
    private void UpdateUpgradeSlot()
    {
        if (upgradeSlot != null)
        {
            // upgradeSlot.SetActive(status == SlotStatus.Unlocked);

        }
    }

    private void Start()
    {
        // Ensure upgrade slot is in the correct state on start
        UpdateUpgradeSlot();
    }
}
