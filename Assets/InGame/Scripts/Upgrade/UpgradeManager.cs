using System.Collections.Generic;
using UnityEngine;
// Include this line if UpgradeManager is outside the Enums namespace

public class UpgradeManager : Singleton<UpgradeManager>
{
    public Level LevelOn = Level.Level_1; // Correctly references the external Level enum

    public GameData gameData; // Reference to the GameData ScriptableObject


    public UpgradeData UpgradeDataValues()
    {
        UpgradeData upgrade = gameData.UpgradeData(LevelOn);

        return upgrade;

    }
}