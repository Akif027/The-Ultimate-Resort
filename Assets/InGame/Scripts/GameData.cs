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