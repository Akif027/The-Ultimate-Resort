using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomData", menuName = "Room Data", order = 1)]

public class GameData : ScriptableObject
{

    public GameObject upgradeEffect;
    public GameObject textEffect;
    public GameObject customerPrefab;

    public List<CharacterData> characterDatas = new List<CharacterData>();
    public List<UpgradeData> upgrades = new List<UpgradeData>();
    public UpgradeData UpgradeData(Level level)
    {
        return upgrades.Find(upgrade => upgrade.Level == level); // Use the public property instead of the field
    }
    public CharacterData CharacterData(CharacterData.Type type)
    {
        return characterDatas.Find((CharacterData cd) => cd.type == type);
    }

    public List<BotResource> botDatas = new List<BotResource>();
    public BotResource BotResource(BotType type)
    {
        return botDatas.Find((BotResource bd) => bd.type == type);
    }

    public CharacterData RandomCharacter
    {
        get
        {
            return characterDatas[Random.RandomRange(0, characterDatas.Count)];
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
    cleaner
}
[System.Serializable]
public class UpgradeData
{
    [SerializeField] // Allows Unity to serialize the field without making it public
    private Level _level; // Keep _level private

    public Level Level => _level; // Public property for read-only access

    public string upgradeName;
    public int cost;
    public int Rooms;
    // Constructor
    private UpgradeData() { }

    // Example method to apply the upgrade
    public void ApplyUpgrade()
    {
        Debug.Log($"Applying upgrade: {upgradeName}");
        // Implement logic to apply the upgrade benefits here
    }
}


public enum Level { Level_1, Level_2, Level_3 }