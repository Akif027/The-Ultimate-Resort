using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : Singleton<Effect>
{
    [SerializeField] GameObject upgradeEffectPrefab; // Assign in Inspector or through script

    void Awake()
    {
        upgradeEffectPrefab = Game.Instance.gameData.upgradeEffect;
    }
    // Method to play an upgrade effect at a specific position
    public void PlayUpgradeEffect(Transform position)
    {
        if (upgradeEffectPrefab != null)
        {
            Instantiate(upgradeEffectPrefab, position.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Upgrade effect prefab is not assigned.");
        }
    }

    // Add more methods as needed for different effects

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
