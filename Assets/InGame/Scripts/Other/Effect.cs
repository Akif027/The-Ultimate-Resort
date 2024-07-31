using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : Singleton<Effect>
{
    // List to be populated in the Inspector
    private Dictionary<string, Queue<GameObject>> effectPools;

    void Awake()
    {
        effectPools = new Dictionary<string, Queue<GameObject>>();
        foreach (var effectData in Game.Instance.gameData.effectList)
        {
            if (!effectPools.ContainsKey(effectData.effectName))
            {
                effectPools.Add(effectData.effectName, new Queue<GameObject>());
            }
        }
    }

    private GameObject GetPooledObject(string effectName, GameObject prefab)
    {
        if (effectPools.TryGetValue(effectName, out Queue<GameObject> pool))
        {
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                return Instantiate(prefab);
            }
        }
        return null;
    }

    public void PlayEffect(string effectName, Vector3 position)
    {
        if (Game.Instance.gameData.effectList.Exists(e => e.effectName == effectName))
        {
            var effectData = Game.Instance.gameData.effectList.Find(e => e.effectName == effectName);
            if (effectData.effectPrefab != null)
            {
                GameObject effectInstance = GetPooledObject(effectName, effectData.effectPrefab);
                if (effectInstance != null)
                {
                    effectInstance.transform.position = position;
                    effectInstance.transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                Debug.LogWarning($"{effectName} effect prefab is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning($"Effect '{effectName}' not found.");
        }
    }

    public void ReturnEffectToPool(string effectName, GameObject effectInstance)
    {
        if (effectPools.TryGetValue(effectName, out Queue<GameObject> pool))
        {
            effectInstance.SetActive(false);
            pool.Enqueue(effectInstance);
        }
    }

    // Example method for text effect
    public void PlayTextEffect(string text, Vector3 position)
    {
        // Example implementation
        GameObject textEffect = new GameObject("TextEffect");
        textEffect.transform.position = position;
        textEffect.AddComponent<TextMesh>().text = text;
        Destroy(textEffect, 2f); // Destroy after 2 seconds as an example
    }
}
