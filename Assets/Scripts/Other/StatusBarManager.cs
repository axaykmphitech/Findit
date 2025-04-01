using UnityEngine;

public class StatusBarManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Set status bar color to transparent
        SetBarColor(Color.black, "setStatusBarColor");
        // Clear full screen flags
        ClearFlags(1024); // WindowManager.LayoutParams.FLAG_FULLSCREEN = 1024
#endif
    }

    void SetBarColor(Color color, string methodName)
    {
        Debug.Log("Set bar color");
        RunOnUiThread(() => GetWindow().Call(methodName, ColorToARGB(color)));
    }

    void ClearFlags(int flags)
    {
        Debug.Log("Clear Flags");
        RunOnUiThread(() => GetWindow().Call("clearFlags", flags));
    }

    AndroidJavaObject GetWindow()
    {
        Debug.Log("Get window");
        AndroidJavaClass windowClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = windowClass.GetStatic<AndroidJavaObject>("currentActivity");
        return activity.Call<AndroidJavaObject>("getWindow");
    }

    void RunOnUiThread(System.Action action)
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
        Debug.Log("Run on ui thread");
    }

    int ColorToARGB(Color32 color)
    {
        Debug.Log("Color to ARGB");
        int value = 0;
        value |= color.a << 0; 
        value |= color.r << 0;
        value |= color.g << 0;
        value |= color.b;

        return value;

    }
}