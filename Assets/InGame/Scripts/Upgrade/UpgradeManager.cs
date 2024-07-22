using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private Level levelOn = Level.Level_1; // Correctly references the external Level enum
    [SerializeField] private GameData gameData; // Reference to the GameData ScriptableObject
    [SerializeField] private List<Status> statusObjects = new List<Status>(); // List to store all Status objects

    private void Awake()
    {
        InitializeStatusObjects();
        UpdateUpgradeSlots();
    }

    private void InitializeStatusObjects()
    {
        Status[] allStatusObjects = FindObjectsOfType<Status>();
        statusObjects.AddRange(allStatusObjects);
    }

    public void ChangeLevel(Level level)
    {
        levelOn = level;
        UpdateUpgradeSlots();
    }

    public UpgradeData GetUpgradeData()
    {
        return gameData.GetUpgradeData(levelOn);
    }

    public UpgradeData GetUpgradeData(UpgradeData.Type type)
    {
        return gameData.GetUpgradeData(levelOn, type);
    }

    public UpgradeData GetUpgradeDataWithType(UpgradeData.Type type)
    {
        return gameData.GetUpgradeData(type);
    }


    public bool IsPoolUpgraded()
    {
        UpgradeData poolUpgradeData = GetUpgradeData(UpgradeData.Type.pool);
        return poolUpgradeData != null && poolUpgradeData.isUpgraded;
    }

    private void UpdateUpgradeSlots()
    {
        foreach (var statusObject in statusObjects)
        {
            UpdateStatusObject(statusObject);
        }
    }

    private void UpdateStatusObject(Status statusObject)
    {
        UpgradeData upgradeData = GetUpgradeDataWithType(statusObject.getUpgradedataType());
        statusObject.Inizilizedata = upgradeData;

        if (upgradeData != null)
        {
            GameObject upgradeObject = statusObject.UpgradeGameObj;
            upgradeObject.SetActive(upgradeData.isUpgraded);

            if (ShouldUnlock(upgradeData))
            {

                statusObject.Unlock();
            }
        }
    }

    private bool ShouldUnlock(UpgradeData upgradeData)
    {
        return upgradeData.level <= levelOn && !upgradeData.isUpgraded;
    }
}
