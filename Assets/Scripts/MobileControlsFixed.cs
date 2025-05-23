using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControlsFixed : MonoBehaviour
{
    [Header("Virtual Joystick")]
    public RectTransform joystickArea;
    public RectTransform joystickHandle;
    public float joystickRange = 50f;
    
    [Header("Touch Buttons")]
    public Button attackButton;
    public Button dashButton;
    public Button ability1Button;
    public Button ability2Button;
    
    [Header("UI Settings")]
    public Canvas mobileUI;
    public float buttonAlpha = 0.8f;
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    public Text debugText;
    
    // Private variables
    private Vector2 joystickInput = Vector2.zero;
    private bool isDragging = false;
    private PlayerController playerController;
    
    // Touch tracking
    private int joystickTouchId = -1;
    
    void Start()
    {
        Debug.Log("[MOBILE] MobileControlsFixed Start() called");
        
        // Get player controller reference
        playerController = FindObjectOfType<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("[MOBILE] PlayerController not found!");
        }
        else
        {
            Debug.Log("[MOBILE] PlayerController found successfully");
        }
        
        // Setup mobile UI
        SetupMobileUI();
        SetupButtons();
        
        Debug.Log("[MOBILE] Mobile Controls Fixed initialized successfully!");
    }
    
    void Update()
    {
        HandleTouchInput();
        SendInputToPlayer();
        
        if (showDebugInfo && debugText != null)
        {
            UpdateDebugInfo();
        }
    }
    
    void SetupMobileUI()
    {
        // Create mobile UI if it doesn't exist
        if (mobileUI == null)
        {
            GameObject uiGO = new GameObject("MobileUI_Fixed");
            mobileUI = uiGO.AddComponent<Canvas>();
            mobileUI.renderMode = RenderMode.ScreenSpaceOverlay;
            mobileUI.sortingOrder = 100;
            
            CanvasScaler scaler = uiGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 1f;
            
            uiGO.AddComponent<GraphicRaycaster>();
        }
        
        CreateVirtualJoystick();
        CreateTouchButtons();
        
        if (showDebugInfo)
        {
            CreateDebugUI();
        }
    }
    
    void CreateVirtualJoystick()
    {
        // Create joystick area
        GameObject joystickAreaGO = new GameObject("JoystickArea");
        joystickAreaGO.transform.SetParent(mobileUI.transform);
        joystickArea = joystickAreaGO.AddComponent<RectTransform>();
        
        joystickArea.anchorMin = new Vector2(0, 0);
        joystickArea.anchorMax = new Vector2(0, 0);
        joystickArea.pivot = new Vector2(0.5f, 0.5f);
        joystickArea.anchoredPosition = new Vector2(150, 150);
        joystickArea.sizeDelta = new Vector2(200, 200);
        
        // Add background
        Image bgImage = joystickAreaGO.AddComponent<Image>();
        bgImage.color = new Color(1, 1, 1, 0.3f);
        bgImage.sprite = CreateCircleSprite();
        
        // Create handle
        GameObject handleGO = new GameObject("JoystickHandle");
        handleGO.transform.SetParent(joystickArea);
        joystickHandle = handleGO.AddComponent<RectTransform>();
        
        joystickHandle.anchorMin = new Vector2(0.5f, 0.5f);
        joystickHandle.anchorMax = new Vector2(0.5f, 0.5f);
        joystickHandle.pivot = new Vector2(0.5f, 0.5f);
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickHandle.sizeDelta = new Vector2(80, 80);
        
        Image handleImage = handleGO.AddComponent<Image>();
        handleImage.color = new Color(1, 1, 1, buttonAlpha);
        handleImage.sprite = CreateCircleSprite();
    }
    
    void CreateTouchButtons()
    {
        // Attack Button (bottom right)
        attackButton = CreateButtonWithCallback("AttackButton", new Vector2(-100, 100), "ATK", 
            () => {
                Debug.Log("[MOBILE] Attack button pressed!");
                if (playerController != null)
                {
                    playerController.PerformMobileAttack();
                    UpdateDebugMessage("Attack pressed!");
                }
            });
        
        // Dash Button (middle right)  
        dashButton = CreateButtonWithCallback("DashButton", new Vector2(-100, 200), "DASH", 
            () => {
                Debug.Log("[MOBILE] Dash button pressed!");
                if (playerController != null)
                {
                    playerController.PerformMobileDash();
                    UpdateDebugMessage("Dash pressed!");
                }
            });
        
        // Ability 1 Button
        ability1Button = CreateButtonWithCallback("Ability1Button", new Vector2(-100, 300), "1", 
            () => {
                Debug.Log("[MOBILE] Ability 1 pressed!");
                if (playerController != null)
                {
                    playerController.UseMobileAbility1();
                    UpdateDebugMessage("Ability 1 pressed!");
                }
            });
        
        // Ability 2 Button
        ability2Button = CreateButtonWithCallback("Ability2Button", new Vector2(-220, 150), "2", 
            () => {
                Debug.Log("[MOBILE] Ability 2 pressed!");
                if (playerController != null)
                {
                    playerController.UseMobileAbility2();
                    UpdateDebugMessage("Ability 2 pressed!");
                }
            });
    }
    
    Button CreateButtonWithCallback(string name, Vector2 position, string text, System.Action callback)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(mobileUI.transform);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(100, 100);
        
        // Background
        Image bgImage = buttonGO.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, buttonAlpha);
        bgImage.sprite = CreateCircleSprite();
        
        // Button component
        Button button = buttonGO.AddComponent<Button>();
        
        // Add callback
        button.onClick.AddListener(() => callback());
        
        // Add visual feedback
        ColorBlock colors = button.colors;
        colors.pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        button.colors = colors;
        
        // Text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        Text textComponent = textGO.AddComponent<Text>();
        textComponent.text = text;
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.fontSize = 28;
        textComponent.fontStyle = FontStyle.Bold;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        Debug.Log($"[MOBILE] Created button: {name} at position {position}");
        
        return button;
    }
    
    void CreateDebugUI()
    {
        GameObject debugGO = new GameObject("DebugText");
        debugGO.transform.SetParent(mobileUI.transform);
        
        RectTransform rect = debugGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(0, -50);
        rect.sizeDelta = new Vector2(-20, 80);
        
        debugText = debugGO.AddComponent<Text>();
        debugText.text = "Mobile Controls Active";
        debugText.color = Color.yellow;
        debugText.fontSize = 20;
        debugText.alignment = TextAnchor.MiddleCenter;
        debugText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void SetupButtons()
    {
        // Buttons already setup in CreateTouchButtons with callbacks
        Debug.Log("[MOBILE] All buttons setup completed");
    }
    
    void HandleTouchInput()
    {
        // Handle multiple touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegan(touch);
                    break;
                    
                case TouchPhase.Moved:
                    HandleTouchMoved(touch);
                    break;
                    
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    HandleTouchEnded(touch);
                    break;
            }
        }
        
        // Mouse input for editor testing
        if (Application.isEditor)
        {
            HandleMouseInput();
        }
    }
    
    void HandleTouchBegan(Touch touch)
    {
        Vector2 localPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea, touch.position, mobileUI.worldCamera, out localPos))
        {
            float distance = Vector2.Distance(localPos, Vector2.zero);
            if (distance <= joystickRange)
            {
                joystickTouchId = touch.fingerId;
                isDragging = true;
                UpdateJoystickHandle(localPos);
                UpdateDebugMessage($"Joystick touched: {touch.fingerId}");
            }
        }
    }
    
    void HandleTouchMoved(Touch touch)
    {
        if (touch.fingerId == joystickTouchId && isDragging)
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickArea, touch.position, mobileUI.worldCamera, out localPos))
            {
                UpdateJoystickHandle(localPos);
            }
        }
    }
    
    void HandleTouchEnded(Touch touch)
    {
        if (touch.fingerId == joystickTouchId)
        {
            joystickTouchId = -1;
            isDragging = false;
            joystickInput = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
            UpdateDebugMessage("Joystick released");
        }
    }
    
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickArea, Input.mousePosition, mobileUI.worldCamera, out localPos))
            {
                float distance = Vector2.Distance(localPos, Vector2.zero);
                if (distance <= joystickRange)
                {
                    isDragging = true;
                    UpdateJoystickHandle(localPos);
                }
            }
        }
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickArea, Input.mousePosition, mobileUI.worldCamera, out localPos))
            {
                UpdateJoystickHandle(localPos);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            joystickInput = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
        }
    }
    
    void UpdateJoystickHandle(Vector2 localPos)
    {
        Vector2 clampedPos = Vector2.ClampMagnitude(localPos, joystickRange);
        joystickHandle.anchoredPosition = clampedPos;
        joystickInput = clampedPos / joystickRange;
    }
    
    void SendInputToPlayer()
    {
        if (playerController != null)
        {
            playerController.SetMobileInput(joystickInput);
        }
    }
    
    void UpdateDebugInfo()
    {
        if (debugText != null)
        {
            string info = $"Joystick: {joystickInput.ToString("F2")}\n";
            info += $"Dragging: {isDragging}\n";
            info += $"Touch Count: {Input.touchCount}";
            debugText.text = info;
        }
    }
    
    void UpdateDebugMessage(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
            Debug.Log($"[DEBUG] {message}");
        }
    }
    
    Sprite CreateCircleSprite()
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2f;
        
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    pixels[y * size + x] = Color.white;
                }
                else
                {
                    pixels[y * size + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
    
    public Vector2 GetJoystickInput()
    {
        return joystickInput;
    }
    
    public bool IsJoystickActive()
    {
        return isDragging;
    }
}