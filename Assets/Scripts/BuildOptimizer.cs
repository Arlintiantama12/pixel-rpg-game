using UnityEngine;

public class BuildOptimizer : MonoBehaviour
{
    [Header("Performance Settings")]
    public int targetFrameRate = 60;
    public bool enableVSync = false;
    
    void Awake()
    {
        // Set target frame rate for mobile
        Application.targetFrameRate = targetFrameRate;
        
        // Disable VSync for better performance
        QualitySettings.vSyncCount = enableVSync ? 1 : 0;
        
        // Set screen orientation to landscape
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        // Optimize for mobile
        QualitySettings.antiAliasing = 0; // Disable AA for better performance
        
        Debug.Log($"Build Optimizer applied: FPS={targetFrameRate}, VSync={enableVSync}");
    }
    
    void Start()
    {
        // Log device info for debugging
        Debug.Log($"Device: {SystemInfo.deviceModel}");
        Debug.Log($"OS: {SystemInfo.operatingSystem}");
        Debug.Log($"RAM: {SystemInfo.systemMemorySize}MB");
        Debug.Log($"GPU: {SystemInfo.graphicsDeviceName}");
        Debug.Log($"Screen: {Screen.width}x{Screen.height}");
    }
}