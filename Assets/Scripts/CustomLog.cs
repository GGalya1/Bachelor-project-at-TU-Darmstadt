using System.Diagnostics;
using Debug = UnityEngine.Debug;

// Debug.Log*s are included in the release build automatically. 
// This produces items on heap and influence runtime.
// To avoid this, all standard Logs are excluded through usage of this class.
public static class CustomLog
{
    [Conditional("UNITY_EDITOR")]
    public static void LogEditor(string message) => Debug.Log(message);

    [Conditional("UNITY_EDITOR")]
    public static void LogEditorWarning(string message) => Debug.LogWarning(message);
    
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(string message) => Debug.Log(message);

    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(string message) => Debug.LogWarning(message);
    
    // Errors must be included. Otherwise, it's hard to detect critical problems
    public static void LogEditorError(string message) => Debug.LogError(message);
}