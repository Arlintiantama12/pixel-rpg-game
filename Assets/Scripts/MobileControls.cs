using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControls : MonoBehaviour
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
    public float buttonAlpha = 0.7f;
    
    // Private variables
    private Vector2 joystickInput = Vector2.zero;
    private bool isDragging = false;
    private Vector2 joystickCenter;
    private PlayerController playerController;
    
    // Touch tracking
    private int joystickTouchId = -1;
    private Dictionary<int, string> activeTouches = new Dictionary<int, string>();
    private bool isInitialized = false;
    
    void Awake()
    {
        // Debug removed - initialization working fine
    }
    
    void Start()
    {
        Debug.Log("[MOBILE] MobileControls Start() called");
        
        // Get player controller reference
        Debug.Log("[MOBILE] Looking for PlayerController...");
        playerController = FindObjectOfType<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogWarning("[MOBILE] PlayerController not found! Creating UI anyway...");
            // Don't return - continue with UI creation
        }
        else
        {
            Debug.Log("[MOBILE] PlayerController found successfully");
        }
        
        // Setup mobile UI
        Debug.Log("[MOBILE] Setting up Mobile UI...");
        SetupMobileUI();
        
        // Setup button events
        Debug.Log("[MOBILE] Setting up button events...");
        SetupButtons();
        
        Debug.Log("[MOBILE] Mobile Controls initialized successfully!");
    }
    
    void Update()
    {
        // Force initialization if Start() never called
        if (!isInitialized)
        {
            Debug.Log("[MOBILE] FORCE INITIALIZATION in Update()!");
            ForceInitialize();
            isInitialized = true;
            return;
        }
        
        // Debug removed - no more loop spam!
        
        HandleTouchInput();
        SendInputToPlayer();
    }
    
    void ForceInitialize()
    {
        Debug.Log("[MOBILE] ForceInitialize() called");
        
        // Get player controller reference
        Debug.Log("[MOBILE] Looking for PlayerController...");
        playerController = FindObjectOfType<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogWarning("[MOBILE] PlayerController not found! Creating UI anyway...");
        }
        else
        {
            Debug.Log("[MOBILE] PlayerController found successfully");
        }
        
        // Setup mobile UI
        Debug.Log("[MOBILE] Setting up Mobile UI...");
        SetupMobileUI();
        
        // Setup button events
        Debug.Log("[MOBILE] Setting up button events...");
        SetupButtons();
        
        Debug.Log("[MOBILE] Force initialization completed!");
    }
    
    void SetupMobileUI()
    {
        Debug.Log("SetupMobileUI called");
        
        // Create mobile UI if it doesn't exist
        if (mobileUI == null)
        {
            Debug.Log("Creating new MobileUI Canvas...");
            GameObject uiGO = new GameObject("MobileUI");
            mobileUI = uiGO.AddComponent<Canvas>();
            mobileUI.renderMode = RenderMode.ScreenSpaceOverlay;
            mobileUI.sortingOrder = 100;
            
            // Add CanvasScaler for responsive UI
            CanvasScaler scaler = uiGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 1f; // Match height for landscape
            
            uiGO.AddComponent<GraphicRaycaster>();
            
            Debug.Log("MobileUI Canvas created successfully");
        }
        
        Debug.Log("Creating Virtual Joystick...");
        CreateVirtualJoystick();
        
        Debug.Log("Creating Touch Buttons...");
        CreateTouchButtons();
        
        Debug.Log("SetupMobileUI completed");
    }
    
    void CreateVirtualJoystick()
    {
        // Create joystick area
        GameObject joystickAreaGO = new GameObject("JoystickArea");
        joystickAreaGO.transform.SetParent(mobileUI.transform);
        joystickArea = joystickAreaGO.AddComponent<RectTransform>();
        
        // Setup joystick area properties
        joystickArea.anchorMin = new Vector2(0, 0);
        joystickArea.anchorMax = new Vector2(0, 0);
        joystickArea.pivot = new Vector2(0.5f, 0.5f);
        joystickArea.anchoredPosition = new Vector2(140, 140);
        joystickArea.sizeDelta = new Vector2(180, 180);
        
        // Add background image
        Image bgImage = joystickAreaGO.AddComponent<Image>();
        bgImage.color = new Color(1, 1, 1, buttonAlpha * 0.5f);
        bgImage.sprite = CreateCircleSprite();
        
        // Create joystick handle
        GameObject handleGO = new GameObject("JoystickHandle");
        handleGO.transform.SetParent(joystickArea);
        joystickHandle = handleGO.AddComponent<RectTransform>();
        
        joystickHandle.anchorMin = new Vector2(0.5f, 0.5f);
        joystickHandle.anchorMax = new Vector2(0.5f, 0.5f);
        joystickHandle.pivot = new Vector2(0.5f, 0.5f);
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickHandle.sizeDelta = new Vector2(70, 70);
        
        // Add handle image
        Image handleImage = handleGO.AddComponent<Image>();
        handleImage.color = new Color(1, 1, 1, buttonAlpha);
        handleImage.sprite = CreateCircleSprite();
        
        joystickCenter = joystickArea.anchoredPosition;
    }
    
    void CreateTouchButtons()
    {
        try
        {
            Debug.Log("Creating touch buttons...");
            
            // Attack Button (bottom right)
            attackButton = CreateButton("AttackButton", new Vector2(-80, 80), "ATK");
            Debug.Log("AttackButton created successfully");
            
            // Dash Button (middle right)
            dashButton = CreateButton("DashButton", new Vector2(-80, 180), "DASH");
            Debug.Log("DashButton created successfully");
            
            // Ability 1 Button (top right)
            ability1Button = CreateButton("Ability1Button", new Vector2(-80, 280), "1");
            Debug.Log("Ability1Button created successfully");
            
            // Ability 2 Button (second column)
            ability2Button = CreateButton("Ability2Button", new Vector2(-180, 130), "2");
            Debug.Log("Ability2Button created successfully");
            
            Debug.Log("All buttons created successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error creating buttons: " + e.Message);
        }
    }
    
    Button CreateButton(string name, Vector2 position, string text)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(mobileUI.transform);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(90, 90);
        
        // Background image
        Image bgImage = buttonGO.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, buttonAlpha);
        bgImage.sprite = CreateCircleSprite();
        
        // Button component
        Button button = buttonGO.AddComponent<Button>();
        
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
        textComponent.fontSize = 24;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        return button;
    }
    
    void SetupButtons()
    {
        if (attackButton != null)
            attackButton.onClick.AddListener(() => OnButtonPressed("attack"));
            
        if (dashButton != null)
            dashButton.onClick.AddListener(() => OnButtonPressed("dash"));
            
        if (ability1Button != null)
            ability1Button.onClick.AddListener(() => OnButtonPressed("ability1"));
            
        if (ability2Button != null)
            ability2Button.onClick.AddListener(() => OnButtonPressed("ability2"));
    }
    
    void HandleTouchInput()
    {
        // Handle multiple touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;
            
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
        
        // Handle mouse input for testing in editor
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
            // Send joystick input to player
            playerController.SetMobileInput(joystickInput);
        }
    }
    
    void OnButtonPressed(string buttonType)
    {
        if (playerController == null) return;
        
        switch (buttonType)
        {
            case "attack":
                playerController.PerformMobileAttack();
                Debug.Log("Mobile Attack pressed!");
                break;
                
            case "dash":
                playerController.PerformMobileDash();
                Debug.Log("Mobile Dash pressed!");
                break;
                
            case "ability1":
                playerController.UseMobileAbility1();
                Debug.Log("Mobile Ability 1 pressed!");
                break;
                
            case "ability2":
                playerController.UseMobileAbility2();
                Debug.Log("Mobile Ability 2 pressed!");
                break;
        }
    }
    
    Sprite CreateCircleSprite()
    {
        // Create a simple circle texture
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
    
    // Enable/disable mobile controls
    public void SetMobileControlsActive(bool active)
    {
        if (mobileUI != null)
            mobileUI.gameObject.SetActive(active);
    }
}