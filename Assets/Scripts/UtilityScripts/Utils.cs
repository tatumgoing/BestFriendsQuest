using MyBox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    private static int sceneToLoad;

    public static readonly float fadeTime = 1f;

    public static float Rand(Vector2 range) => Random.Range(range.x, range.y);

    private static int AXIS_BITS = 6;
    private static int AXIS_STEPS = 1 << AXIS_BITS;

    public static int MenusOpen { get; private set; }

    /// <summary>
    /// if there is a menu open, the mouse is visible and unlocked. if there are no menus open, the mouse is hidden and locked.
    /// by default, there is 1 menu open.
    /// when opening the pause menu, that's a second menu that's open
    /// no menus is for things like freecam in the park
    /// except for the townGameManager, use only openMenu and closeMenu. only the gm is allowed to use SetMenus, and only to 1 when it starts.
    /// </summary>
    /// <param name="count"></param>
    public static void SetMenus(int count) => MenusOpen = count;

    public static void OpenMenu()
    {
        //Debug.Log("opened Menu");
        MenusOpen += 1;
        SetCursor(MenusOpen > 0);
    }

    public static void CloseMenu()
    { 
        //Debug.Log("closed Menu");
        MenusOpen = Mathf.Max(0, MenusOpen - 1);
        SetCursor(MenusOpen > 0);
    }

    private static void SetCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static float CompareColors(Color col1, Color col2)
    {
        var rDiff = Mathf.Abs(col1.r - col2.r);    
        var gDiff = Mathf.Abs(col1.g - col2.g);
        var bDiff = Mathf.Abs(col1.b - col2.b);

        return rDiff + gDiff + bDiff;
    }

    public static Color DarkerMoreSaturatedMoreRed(Color c, float redShift = 0.15f, float saturationBoost = 0.2f, float valueMultiplier = 0.7f)
    {
        Color.RGBToHSV(c, out float h, out float s, out float v);

        // Move hue toward red (0 or 1 — whichever is closer)
        float targetRed = (h > 0.5f) ? 1f : 0f;
        h = Mathf.Lerp(h, targetRed, redShift);

        // Increase saturation and darken
        s = Mathf.Clamp01(s + saturationBoost);
        v = Mathf.Clamp01(v * valueMultiplier);

        return Color.HSVToRGB(h, s, v);
    }

    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#")) hex = hex.Substring(1);
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    public static int QuantizeAngle(float degrees)
    {
        degrees = Mathf.Repeat(degrees, 360f);

        int q = Mathf.RoundToInt( degrees / 360f * (AXIS_STEPS - 1) );
        
        // prevent negative or overflow:
        return Mathf.Clamp(q, 0, AXIS_STEPS - 1);
    }

    public static float DequantizeAngle(int q)
    {
        return q / (float) (AXIS_STEPS - 1) * 360f;
    }

    public static string EncodeQuaternions12(Quaternion a, Quaternion b)
    {
        Vector3 eulerA = a.eulerAngles;
        Vector3 EuerlB = b.eulerAngles;

        ulong packed = 0;
        int shift = 0;

        void Pack(float angle)
        {
            if (shift >= 36)
                throw new System.Exception("Quaternion pack overflow");

            ulong q = (ulong)QuantizeAngle(angle); // 0–63
            packed |= q << shift;
            shift += 6;
        }

        Pack(eulerA.x);
        Pack(eulerA.y);
        Pack(eulerA.z);
        Pack(EuerlB.x);
        Pack(EuerlB.y);
        Pack(EuerlB.z);

        // This value is guaranteed < 2^36 ≈ 6.87e10
        // Which fits comfortably in 12 digits
        return packed.ToString("D12");
    }

    public static void DecodeQuaternions12(string inputString, out Quaternion QuaternionA, out Quaternion QuaternionB)
    {
        ulong packed = ulong.Parse(inputString);
        int shift = 0;

        float UnpackAxis()
        {
            int q = (int)((packed >> shift) & (ulong)(AXIS_STEPS - 1));
            shift += AXIS_BITS;
            return DequantizeAngle(q);
        }

        Vector3 eulerA = new Vector3(
            UnpackAxis(),
            UnpackAxis(),
            UnpackAxis()
        );

        Vector3 eulerB = new Vector3(
            UnpackAxis(),
            UnpackAxis(),
            UnpackAxis()
        );

        QuaternionA = Quaternion.Euler(eulerA);
        QuaternionB = Quaternion.Euler(eulerB);
    }

    public static string CapitalFirst(string input)
    {
        var firstLetter = input[0].ToString().ToUpper();
        return firstLetter + input[1..].ToLower();
    }

    public static string EnumInt<T>(T enumValue) {
        return (System.Convert.ToInt32(enumValue)).ToString();
    }

    public static T IntEnum<T>(string intString)
    {
        var num = int.Parse(intString);
        return (T) (object) num;
    }

    public static List<T> EnumToList<T>()
    {
        var array = System.Enum.GetValues(typeof(T));
        var list = new List<T>();
        foreach (var item in array) list.Add((T)item);
        return list;
    }

    public static string GetTimeString(int seconds)
    {
        seconds = Mathf.FloorToInt(seconds);
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(seconds);
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        return timeString;
    }

    private static void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public static void SetDirty(Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }

    public static string Vec2String(Vector2 v) => v.x + ":" + v.y;
}
