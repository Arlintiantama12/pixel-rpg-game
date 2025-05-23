using UnityEngine;
using UnityEngine.UI;

public class MobileDebugTester : MonoBehaviour
{
    [Header("Debug UI")]
    public Text statusText;
    public Text playerStatsText;
    public Text inputText;
    
    private PlayerController playerController;
    private Canvas debugCanvas;
    
    void Start()
    {
        // Find player controller
        playerController = FindObjectOfType<PlayerController>();
        
        // Create debug UI
        CreateDebugUI();
        
        // Update status
        if (playerController != null)
        {
            UpdateStatus("PlayerController found! Mobile test ready.");
        }
        else
        {
            UpdateStatus("ERROR: PlayerController not found!");
        }
    }
    
    void Update()
    {
        UpdateDebugInfo();
        
        // Test keyboard inputs too
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestAttack();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TestDash();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            TestAbility1();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            TestAbility2();
        }
    }
    
    void CreateDebugUI()
    {
        // Create canvas if not exists
        debugCanvas = FindObjectOfType<Canvas>();
        if (debugCanvas == null)
        {
            GameObject canvasGO = new GameObject("DebugCanvas");
            debugCanvas = canvasGO.AddComponent<Canvas>();
            debugCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            debugCanvas.sortingOrder = 200; // Higher than mobile UI
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        
        // Status text (top)
        CreateText("StatusText", new Vector2(0, -30), ref statusText, "Debug Status", Color.yellow);
        
        // Player stats (top left)
        CreateText("PlayerStatsText", new Vector2(10, -100), ref playerStatsText, "Player Stats", Color.cyan);
        
        // Input info (top right)
        CreateText("InputText", new Vector2(-10, -100), ref inputText, "Input Info", Color.green);
        inputText.alignment = TextAnchor.UpperRight;
        
        // Test buttons
        CreateTestButtons();
    }
    
    void CreateText(string name, Vector2 position, ref Text textRef, string content, Color color)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(debugCanvas.transform);
        
        RectTransform rect = textGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(-20, 80);
        
        textRef = textGO.AddComponent<Text>();
        textRef.text = content;
        textRef.color = color;
        textRef.fontSize = 18;
        textRef.alignment = TextAnchor.UpperLeft;
        textRef.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void CreateTestButtons()
    {
        // Test Attack Button
        CreateTestButton("TestAttack", new Vector2(50, 50), "TEST ATK", Color.red, TestAttack);
        
        // Test Dash Button  
        CreateTestButton("TestDash", new Vector2(200, 50), "TEST DASH", Color.blue, TestDash);
        
        // Test Ability 1
        CreateTestButton("TestAbility1", new Vector2(350, 50), "TEST AB1", Color.green, TestAbility1);
        
        // Test Ability 2
        CreateTestButton("TestAbility2", new Vector2(500, 50), "TEST AB2", Color.magenta, TestAbility2);
    }
    
    void CreateTestButton(string name, Vector2 position, string text, Color color, System.Action callback)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(debugCanvas.transform);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(120, 60);
        
        Image bgImage = buttonGO.AddComponent<Image>();
        bgImage.color = color;
        
        Button button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(() => callback());
        
        // Button text
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
        textComponent.fontSize = 16;
        textComponent.fontStyle = FontStyle.Bold;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void UpdateDebugInfo()
    {
        if (playerController != null)
        {
            // Player stats
            string stats = $"Class: {playerController.characterClass}\\n";
            stats += $"Level: {playerController.level}\\n";
            stats += $"HP: {playerController.currentHealth}/{playerController.maxHealth}\\n";
            stats += $"MP: {playerController.currentMana}/{playerController.maxMana}\\n";
            stats += $"ATK: {playerController.attackPower}\\n";
            stats += $"DEF: {playerController.defense}";
            
            if (playerStatsText != null)
                playerStatsText.text = stats;
        }
        
        // Input info
        string inputInfo = $"Touch Count: {Input.touchCount}\\n";
        inputInfo += $"Mouse Pos: {Input.mousePosition}\\n";
        inputInfo += $"Screen: {Screen.width}x{Screen.height}\\n";
        inputInfo += "Test Keys: T=ATK, Y=DASH, U=AB1, I=AB2";
        
        if (inputText != null)
            inputText.text = inputInfo;
    }
    
    public void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log($"[DEBUG] {message}");
    }
    
    public void TestAttack()
    {
        if (playerController != null)
        {
            playerController.PerformMobileAttack();
            UpdateStatus("✅ ATTACK TEST - Called PerformMobileAttack()");
        }
        else
        {
            UpdateStatus("❌ ATTACK TEST FAILED - No PlayerController");
        }
    }
    
    public void TestDash()
    {
        if (playerController != null)
        {
            playerController.PerformMobileDash();
            UpdateStatus("✅ DASH TEST - Called PerformMobileDash()");
        }
        else
        {
            UpdateStatus("❌ DASH TEST FAILED - No PlayerController");
        }
    }
    
    public void TestAbility1()
    {
        if (playerController != null)
        {
            playerController.UseMobileAbility1();
            UpdateStatus("✅ ABILITY 1 TEST - Called UseMobileAbility1()");
        }
        else
        {
            UpdateStatus("❌ ABILITY 1 TEST FAILED - No PlayerController");
        }
    }
    
    public void TestAbility2()
    {
        if (playerController != null)
        {
            playerController.UseMobileAbility2();
            UpdateStatus("✅ ABILITY 2 TEST - Called UseMobileAbility2()");
        }
        else
        {
            UpdateStatus("❌ ABILITY 2 TEST FAILED - No PlayerController");
        }
    }
}