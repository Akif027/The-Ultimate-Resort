using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Resource", order = 1)]
public class GameData : ScriptableObject
{
    public SecuredDouble money;
    public GameObject upgradeEffect;
    public GameObject textEffect;
    public GameObject customerPrefab;
    public List<LevelData> levels = new List<LevelData>();
    public List<CharacterData> characterDatas = new List<CharacterData>();
    public List<UpgradeData> upgrades = new List<UpgradeData>();
    public List<BotResource> botDatas = new List<BotResource>();

    private Dictionary<Level, UpgradeData> upgradeDictionary = new Dictionary<Level, UpgradeData>();

    private void OnEnable()
    {


        // Populate the dictionary when the GameData is enabled
        foreach (var upgrade in upgrades)
        {
            upgradeDictionary[upgrade.level] = upgrade;
        }
    }
    void Start()
    {


    }
    public void AddMoney(SecuredDouble value)
    {
        money += value;
        EventManager.RaiseOnCoinChanged();
    }

    public void SpendMoney(SecuredDouble value)
    {
        money -= value;
        EventManager.RaiseOnCoinChanged();
    }

    public bool TrySpendMoney(SecuredDouble value)
    {
        if (money >= value)
        {
            money -= value;
            EventManager.RaiseOnCoinChanged();
            return true;
        }
        return false;
    }

    public UpgradeData GetUpgradeData(Level level)
    {
        return upgradeDictionary.TryGetValue(level, out var upgradeData) ? upgradeData : null;
    }
    public UpgradeData GetUpgradeData(Level level, UpgradeData.Type type)
    {
        return upgrades.Find(upgrade => upgrade.level == level && upgrade.type == type);
    }

    public LevelData GetLevelData(Level type)
    {
        foreach (var le in levels)
        {
            if (le.level == type)
            {
                return le;
            }
        }
        return null; // Return null if no upgrade matches the specified type
    }



    public CharacterData GetCharacterData(CharacterData.Type type)
    {
        return characterDatas.Find(cd => cd.type == type);
    }

    public BotResource GetBotResource(BotType type)
    {
        return botDatas.Find(bd => bd.type == type);
    }

    public CharacterData RandomCharacter
    {
        get
        {
            return characterDatas[Random.Range(0, characterDatas.Count)];
        }
    }
}

[System.Serializable]
public class CharacterData
{
    public enum Type { Player, Customer, Bot }

    public Type type;
    public GameObject[] model;

    public GameObject GetRandomModel()
    {
        return model[Random.Range(0, model.Length)];
    }
}

[System.Serializable]
public class BotResource
{
    public BotType type;
    public GameObject model;
}

[System.Serializable]
public class MapData
{
    public GameObject model;
}

public enum BotType
{
    Cleaner
}

[System.Serializable]
public class LevelData
{
    public Level level;
    public int rooms;
}

[System.Serializable]
public class UpgradeData
{
    public enum Type { Room, Room2 }
    public Type type;
    public Level level;
    public SecuredDouble cost;
    public bool isUpgraded = false;

    public void SpendCost(SecuredDouble value)
    {
        if (cost > 0)
        {
            cost -= value;
        }
    }
}

public enum Level { Level_1, Level_2, Level_3 }
public enum IngredientType { ToiletPaper }