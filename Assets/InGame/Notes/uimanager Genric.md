# uimanager Genric

using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Manager
{
public static UIManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> holdUIElements = new List<GameObject>();

    [SerializeField]
    private List<GameObject> roofLayer = new List<GameObject>();

    private Dictionary<string, UnityEngine.Object> uiElements = new Dictionary<string, UnityEngine.Object>();

    private RoomManager roomManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Init()
    {
        // Any initialization logic specific to UIManager can go here
    }

    void Start()
    {
        roomManager = GameManager.Instance.GetManager<RoomManager>() as RoomManager;

        InitializeUIElements(holdUIElements);
        DebugUIElements();
        InitializeRoomRoofElements();

    }
    public void DebugUIElements()
    {
        foreach (var kvp in uiElements)
        {
            Debug.Log($"UI Element Key: {kvp.Key}, GameObject: {kvp.Value?.name}");
        }
    }
    void Update()
    {
        //GameObject textElement = GetUIElement<GameObject>("AcceptCustomerB");
        // textElement.SetActive(false);
    }
    private void InitializeUIElements(IEnumerable<GameObject> elements)
    {
        foreach (var uiObject in holdUIElements)
        {
            string uiId = uiObject.name; // Using the GameObject's name as the key
            if (!uiElements.ContainsKey(uiId))
            {
                uiElements.Add(uiId, uiObject);
            }
            else
            {
                Debug.LogWarning($"UI element with ID '{uiId}' already exists.");
            }
        }

    }

    private void InitializeRoomRoofElements()
    {
        for (int i = 0; i < roomManager.room.Count && i < roofLayer.Count; i++)
        {
            Room room = roomManager.room[i];
            string uiId = $"roomRoof{room.RoomNumber}";

            if (uiElements.ContainsKey(uiId))
            {
                Debug.LogWarning($"UI element with ID '{uiId}' already exists.");
                continue;
            }

            AddUIElement(uiId, roofLayer[i]);
        }
    }
    public T GetUIElement<T>(string elementName) where T : UnityEngine.Object
    {
        if (uiElements.TryGetValue(elementName, out UnityEngine.Object obj))
        {
            return obj as T;
        }
        else
        {
            Debug.LogError($"Element '{elementName}' not found in the dictionary.");
            return null;
        }
    }
    private void AddUIElement(string name, GameObject element)
    {
        if (!uiElements.ContainsKey(name))
        {
            uiElements.Add(name, element);
            Debug.Log($"Added UI element: {name}");
        }
        else
        {
            Debug.LogWarning($"UI Element with name '{name}' already exists.");
        }
    }
    public void HideUIElement(string elementName)
    {
        SetUIElementActive(elementName, false);
    }

    public void ShowUIElement(string elementName)
    {
        SetUIElementActive(elementName, true);
    }
    private void SetUIElementActive(string elementName, bool isActive)
    {
        GameObject element = GetUIElement<GameObject>(elementName);
        if (element != null)
        {
            Debug.LogError($"Element '{elementName}' could not be retrieved.");
            return;
        }
        element.SetActive(isActive);
    }
    public void UpdateUIElement<T>(string elementName, string newValue) where T : UnityEngine.Object
    {
        T element = GetUIElement<T>(elementName);
        if (element is TMP_Text textElement)
        {
            textElement.text = newValue;
        }
        else
        {
            Debug.LogError($"Element with name '{elementName}' is not a supported text element.");
        }
    }

    private void OnDestroy()
    {
        uiElements.Clear();
        holdUIElements.Clear();
        roofLayer.Clear();
    }

}
