using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private Level levelOn; // Correctly references the external Level enum
    [SerializeField] private GameData gameData; // Reference to the GameData ScriptableObject
    [SerializeField] private List<Status> statusObjects = new List<Status>(); // List to store all Status objects

    [SerializeField] ReceptionManager ReceptionManager;


    private void Awake()
    {
        levelOn = gameData.GetCurretLevel();

        InitializeStatusObjects();
        UpdateUpgradeSlots();


    }
    void Update()
    {
        #region HandleLevelLogic
        if (ReceptionManager.CustomerHandled == (int)levelOn * 10) //after Level(1,2,3) * 10
        {
            IncreseLevel();
        }

        #endregion
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

    public void IncreseLevel()
    {
        int nextLevelValue = ((int)levelOn + 1);

        if (nextLevelValue <= Settings.LevelMax) // Assuming Level_3 is the highest level
        {
            levelOn = (Level)nextLevelValue;
            UpdateUpgradeSlots();
        }
        else
        {
            Debug.LogWarning("Cannot increase level beyond the maximum.");
        }

    }

    public Level CurrentLevel()
    {
        return levelOn;
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
