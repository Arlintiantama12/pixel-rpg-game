using UnityEngine;
using UnityEngine.UI;

public class TestGameUI : MonoBehaviour
{
    [Header("UI References")]
    public Text statusText;
    public Text fpsText;
    public Text playerInfoText;
    
    private PlayerController playerController;
    private float fpsTimer;
    private int frameCount;
    private float fps;
    
    void Start()
    {
        // Find player controller
        playerController = FindObjectOfType<PlayerController>();
        
        // Create UI if not assigned
        if (statusText == null) CreateStatusUI();
        if (fpsText == null) CreateFPSUI();
        if (playerInfoText == null) CreatePlayerInfoUI();
        
        UpdateStatusText("Game Started - Touch controls active!");
    }
    
    void Update()
    {
        UpdateFPS();
        UpdatePlayerInfo();
    }
    
    void CreateStatusUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO;
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        
        GameObject textGO = new GameObject("StatusText");
        textGO.transform.SetParent(canvas.transform);
        
        RectTransform rect = textGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(0, -30);
        rect.sizeDelta = new Vector2(-20, 50);
        
        statusText = textGO.AddComponent<Text>();
        statusText.text = "Pixel RPG - APK Test Build";
        statusText.color = Color.white;
        statusText.fontSize = 24;
        statusText.alignment = TextAnchor.MiddleCenter;
        statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void CreateFPSUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        
        GameObject fpsGO = new GameObject("FPSText");
        fpsGO.transform.SetParent(canvas.transform);
        
        RectTransform rect = fpsGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(10, -10);
        rect.sizeDelta = new Vector2(100, 30);
        
        fpsText = fpsGO.AddComponent<Text>();
        fpsText.text = "FPS: 60";
        fpsText.color = Color.green;
        fpsText.fontSize = 18;
        fpsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void CreatePlayerInfoUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        
        GameObject infoGO = new GameObject("PlayerInfoText");
        infoGO.transform.SetParent(canvas.transform);
        
        RectTransform rect = infoGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(10, 150);
        rect.sizeDelta = new Vector2(300, 100);
        
        playerInfoText = infoGO.AddComponent<Text>();
        playerInfoText.text = "Player Info";
        playerInfoText.color = Color.cyan;
        playerInfoText.fontSize = 16;
        playerInfoText.alignment = TextAnchor.UpperLeft;
        playerInfoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void UpdateFPS()
    {
        frameCount++;
        fpsTimer += Time.deltaTime;
        
        if (fpsTimer >= 1f)
        {
            fps = frameCount / fpsTimer;
            frameCount = 0;
            fpsTimer = 0f;
            
            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps:F0}";
                fpsText.color = fps >= 50 ? Color.green : fps >= 30 ? Color.yellow : Color.red;
            }
        }
    }
    
    void UpdatePlayerInfo()
    {
        if (playerController != null && playerInfoText != null)
        {
            string info = $"Class: {playerController.characterClass}\n";
            info += $"Level: {playerController.level}\n";
            info += $"HP: {playerController.currentHealth}/{playerController.maxHealth}\n";
            info += $"MP: {playerController.currentMana}/{playerController.maxMana}";
            
            playerInfoText.text = info;
        }
    }
    
    public void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            Debug.Log($"[UI] {message}");
        }
    }
    
    // Test buttons
    public void OnTestAttack()
    {
        UpdateStatusText("Attack button working!");
    }
    
    public void OnTestDash()
    {
        UpdateStatusText("Dash button working!");
    }
}