using System;
using System.Diagnostics;
using UObject = UnityEngine.Object;


public class Develop
{
    public delegate void FLogFormat(string format, params object[] args);

    public static bool IsLogEnabled
    {
        get
        {
            return true;
        }
    }

    static Develop()
    {
        if (!UnityEngine.PlayerPrefs.HasKey("WriteLogFlag"))
        {
            UnityEngine.PlayerPrefs.SetInt("WriteLogFlag", 0);      //默认关闭日志
        }
        Update();
    }

    public static void Update()
    {
        bool isEnable = UnityEngine.PlayerPrefs.GetInt("WriteLogFlag") == 1;

        Log = isEnable ? UnityEngine.Debug.Log : LogEmpty;
        LogF = isEnable ? UnityEngine.Debug.LogFormat : LogFEmpty;
        LogWarning = isEnable ? UnityEngine.Debug.LogWarning : LogEmpty;
        LogWarningF = isEnable ? UnityEngine.Debug.LogWarningFormat : LogFEmpty;

    }

    public static Action<object> LogEmpty = (o) => { };
    public static FLogFormat LogFEmpty = (o, a) => { };

    public static Action<object /*message*/> Log = UnityEngine.Debug.Log;
    public static FLogFormat LogF = UnityEngine.Debug.LogFormat;
    public static Action<object /*message*/> LogWarning = UnityEngine.Debug.LogWarning;
    public static FLogFormat LogWarningF = UnityEngine.Debug.LogWarningFormat;

    public static Action<object /*message*/> LogError = UnityEngine.Debug.LogError;
    public static FLogFormat LogErrorF = UnityEngine.Debug.LogErrorFormat;

    public static Action<Exception /*exception*/> LogException = UnityEngine.Debug.LogException;
}

