using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Manager
{
    public static UIManager Instance { get; private set; } // Singleton instance
    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddUIElement(string name, GameObject element)
    {
        if (!uiElements.ContainsKey(name))
        {
            uiElements.Add(name, element);

        }
        else
        {
            Debug.LogWarning($"UI Element with name '{name}' already exists.");
        }
    }


    public T GetUIElement<T>(string elementName) where T : UnityEngine.Object
    {
        if (!uiElements.ContainsKey(elementName))
        {
            var foundObject = FindObjectOfType(typeof(T)) as T;
            if (foundObject != null)
            {
                var gameObject = (foundObject as Component)?.gameObject;
                if (gameObject != null)
                {
                    uiElements.Add(elementName, gameObject);
                }
                else
                {
                    Debug.LogError($"UI Element with name '{elementName}' not found.");
                    return default(T);
                }
            }
            else
            {
                Debug.LogError($"UI Element with name '{elementName}' not found.");
                return default(T);
            }
        }

        return uiElements[elementName] as T;
    }
    // Example usage
    public TMP_Text ScoreText => GetUIElement<TMP_Text>("ScoreText");
    public void ShowUIElement(string elementName)
    {
        var element = GetUIElement<GameObject>(elementName);
        if (element != null)
        {
            element.SetActive(true);
        }
    }

    public void HideUIElement(string elementName)
    {
        var element = GetUIElement<GameObject>(elementName);
        if (element != null)
        {
            element.SetActive(false);
        }
    }

    public void UpdateUIElement<T>(string elementName, string newValue) where T : UnityEngine.Object
    {
        var element = GetUIElement<T>(elementName);
        if (element != null)
        {
            if (element is TMP_Text uiText)
            {
                uiText.text = newValue;
            }
            else if (element is TMP_Text tmpText)
            {
                tmpText.text = newValue;
            }
            else
            {
                Debug.LogError($"Element with name '{elementName}' is not a supported text element.");
            }
        }
    }
}
