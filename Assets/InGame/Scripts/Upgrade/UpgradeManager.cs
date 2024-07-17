using System.Collections.Generic;
using UnityEngine;
// Include this line if UpgradeManager is outside the Enums namespace

public class UpgradeManager : Singleton<UpgradeManager>
{
    public Level LevelOn = Level.Level_1; // Correctly references the external Level enum

    public GameData gameData; // Reference to the GameData ScriptableObject
    public float scaleUpDuration = 0.5f;
    public float scaleDownDuration = 0.5f;
    public float scaleMultiplier = 1.5f;
    public Color upgradeColor = Color.yellow;


    //based on the level and what the player has already upgraded we will enable those in this Script <--

    public UpgradeData UpgradeDataValues()
    {
        UpgradeData upgrade = gameData.GetUpgradeData(LevelOn);

        return upgrade;

    }



    public UpgradeData UpgradeDataValues(UpgradeData.Type type)
    {
        UpgradeData upgrade = gameData.GetUpgradeData(LevelOn, type);

        return upgrade;

    }

    public LevelData GetLevelDataValues()
    {
        LevelData levelD = gameData.GetLevelData(LevelOn);

        return levelD;

    }
    public void UpgradeRoom(Transform t, Transform Effectpos)
    {
        AllPurposeCamera.Instance.FocusOnTarget(t);
        DoTweenManager.PlayUpgradeAnimation(t, scaleUpDuration, scaleDownDuration, scaleMultiplier, upgradeColor, gameData.upgradeEffect, Effectpos);
    }
}